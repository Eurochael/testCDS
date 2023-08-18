Imports System.Net.Sockets
Imports System.Threading
Imports System
Imports System.Text                 '문자 인코딩을 위함
Imports System.IO
Imports System.Net
Imports System.Collections
Imports System.Collections.Concurrent

Public Class Cls_Socket
    Implements IDisposable
#Region "Initial"
    Private Const Int_bufferSize As Integer = 2048
    Private Const Int_CM_Max_Count As Integer = 50
    Private server_port As Integer = 5000
    Private Const Int_Interval_PingTest As Integer = 5000

    Private TL_Server As New TcpListener(server_port)
    Private TC_CM(Int_CM_Max_Count) As TcpClient

    Public ReadOnly Socket_CM(Int_CM_Max_Count) As Socket '통신간 사용할 소켓
    Public dt_last_SendTime_Socket(Int_CM_Max_Count) As DateTime '마지막 전송 시간
    'Public bol_Socket_ConnectedStatus(Int_CM_Max_Count) As Boolean '마지막 전송 시간
    Private byte_DataBuffer(Int_CM_Max_Count)() As Byte '통신간 Recieve데이타를 저장하는 Object 변수

    Public thd_WaitClient As Thread = New Threading.Thread(AddressOf StartListen)          '클라이언트의 연결 요청을 기다리는 스레드
    Public Thd_PingTest As Thread = New Threading.Thread(AddressOf HashTable_PingTest) '설정된 Interval로 Ping 확인 스레드
    Public Thd_MainThread As Thread = New Threading.Thread(AddressOf Manage_Thread) '각각의 스레드 관리 및 실행
    Public CM_HashTable As New Hashtable '통신간 사용되는 IP주소 및 포트간 실시간으로 담당하는 DataTable '127.0.0.1:5000|0 과같이 IP:Port|Sock_Num으로 관리된다
    Public Col_Value As New Collection '클라이언트 요청 및 요구에 대하여 Socket_Num을 할당하기 위한 컬렉션

    Private obj_Monitor_ConnectionClose As New Object
    Private Obj_Monitor_Thd_MainThread As New Object
    Private Obj_Monitor_Thd_PingTest As New Object
    Private Obj_Monitor_Thd_WaitClient As New Object
    Private Obj_Monitor_ConnectionOpen As New Object

    Private Obj_Monitor_CallBack_Recieve As New Object
    Private Obj_Monitor_CallBack_Connect As New Object
    Private Obj_Monitor_CallBack_Send As New Object

    Private Event Ev_Thread_RecieveCompleted(ByVal Async_Result As IAsyncResult, ByVal Int_DataLength As Integer)
    Private Event Ev_Thread_SendCompleted(ByVal Sender As Socket_Connection_Info, ByVal Bt_DataByte() As Byte)
    Private Event Ev_Thread_Error(ByVal Async_Result As IAsyncResult)
    Private Event Ev_Thread_Connect(ByVal Sender As Socket_Connection_Info, ByVal str_Type As String, ByVal BT_InitialByte() As Byte)
    Private Event Ev_Thread_Disconnect_Async(ByVal Async_Result As IAsyncResult)
    Private Event Ev_Thread_Disconnect_Object(ByVal Async_Result As Socket_Connection_Info)

    Private disposedValue As Boolean = False ' 중복 호출을 검색하려면

    'Socket에서 발생하는 open, close, rcv, send 등의 이벤트 포함 큐
    'use_queue_send_event = true시에만 send 이벤트 큐에 담음
    'use_queue_send_event = false시에는 send 이벤트 큐에 담지 않음
    Public m_Col_Socket_Event_List As ConcurrentQueue(Of Socket_Event) = New ConcurrentQueue(Of Socket_Event)()
    Public m_Col_Error_List As Queue(Of String) = New Queue(Of String)
    Public use_queue_send_event As Boolean = True

    Public ctc_connected_ip As String = ""
    Public ctc_connected_port As Integer = 0
    Public listbox As New Collection()
    Public Structure Socket_Connection_Info
        Public ip As String
        Public port As Integer
        Public idx_socket_cm As Integer
        Public dt_occur As DateTime
        'Client Mode에서 접속과 동시에 데이터를 보낼때 사용 변수
        Public connect_and_send_data_to_string As String
        Public connect_and_send_data_to_byte() As Byte
        Public req_clear_hashtableinfo_by_server As String '개별 삭제 시 for each hashtable 접근 불가로, server에서 삭제시에는 별도로 hashtable clear
    End Structure

    'Socket Event 관리 구조체 / Open, Close, Rcv 등 정보 관리
    Public Structure Socket_Event
        Public status As Socket_Event_Type
        Public dt_occur As DateTime
        Public info_ip As String
        Public info_port As Integer
        Public info_ip_port_type1 As String
        Public data_length As Integer
        Public data_to_array() As Byte
        Public data_to_string As String
    End Structure
    Public Enum Socket_Event_Type
        None = 0
        Open = 10
        Close = 20
        Recieve = 30
        Send = 40
    End Enum
    Private Sub FrmCommunication_InitialSetting()
        '컬렉션 추가 Value, Key값
        Col_Value.Clear()
        For i As Integer = 0 To Int_CM_Max_Count - 1
            Col_Value.Add(i, CInt(i))
        Next
        Del_Handler()
        Add_Handler()
        'ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub Add_Handler()
        '이벤트 핸들러 추가
        AddHandler Ev_Thread_RecieveCompleted, AddressOf Recive_Data
        AddHandler Ev_Thread_SendCompleted, AddressOf Send_Completed
        AddHandler Ev_Thread_Error, AddressOf Connection_Error
        AddHandler Ev_Thread_Connect, AddressOf Connection_Open
        AddHandler Ev_Thread_Disconnect_Async, AddressOf Connection_Close_Async
        AddHandler Ev_Thread_Disconnect_Object, AddressOf Connection_Close_Object
    End Sub
    Private Sub Del_Handler()
        RemoveHandler Ev_Thread_RecieveCompleted, AddressOf Recive_Data
        RemoveHandler Ev_Thread_SendCompleted, AddressOf Send_Completed
        RemoveHandler Ev_Thread_Error, AddressOf Connection_Error
        RemoveHandler Ev_Thread_Connect, AddressOf Connection_Open
        RemoveHandler Ev_Thread_Disconnect_Async, AddressOf Connection_Close_Async
        RemoveHandler Ev_Thread_Disconnect_Object, AddressOf Connection_Close_Object
    End Sub

#End Region

    Private int_Socket_Mode As enum_Socket_mode = 0
    Public Enum enum_Socket_mode
        NONE = 0
        SEMES = 5
        CSE_AMP = 10
    End Enum
    Public Sub New(ByVal int_Mode As enum_Socket_mode, ByVal int_PortNo As Integer)
        Try
            int_Socket_Mode = int_Mode
            FrmCommunication_InitialSetting()

            If int_Socket_Mode <> enum_Socket_mode.CSE_AMP Then
                server_port = int_PortNo
                Thd_MainThread.Start()
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
        End Try

    End Sub
    Public Sub New(ByVal int_PortNo As Integer)
        Try
            int_Socket_Mode = enum_Socket_mode.NONE
            FrmCommunication_InitialSetting()
            server_port = int_PortNo
            Thd_MainThread.Start()
        Catch ex As Exception
            error_refresh(ex.ToString())
        End Try
    End Sub
    Public Function Socket_Info() As String
        Dim result As String = ""
        For i As Integer = 0 To Int_CM_Max_Count
            If Not Socket_CM(i) Is Nothing Then
                result = "," & i & "-" & Socket_CM(i).RemoteEndPoint.ToString()
            End If
        Next
        Return result
    End Function
    Public Sub New()

    End Sub
    Public Function Server_Start(ByVal int_PortNo As Integer) As String
        Dim result As String = ""
        Try
            int_Socket_Mode = enum_Socket_mode.NONE
            Server_Stop()
            FrmCommunication_InitialSetting()
            server_port = int_PortNo
            If Not Thd_MainThread Is Nothing AndAlso Thd_MainThread.IsAlive = False Then
                'Manage_Thread 내부에서 client thread 실행
                Thd_MainThread.Abort()
                Thd_MainThread = New Thread(AddressOf Manage_Thread)
                Thd_MainThread.Start()
            ElseIf Thd_MainThread Is Nothing Then
                Thd_MainThread = New Thread(AddressOf Manage_Thread)
                Thd_MainThread.Start()
            End If
            result = ""
        Catch ex As Exception
            error_refresh(ex.ToString())
            result = ex.ToString()
        Finally

        End Try
        Return result
    End Function
    Public Function Client_Start() As String
        Dim result As String = ""
        Try
            int_Socket_Mode = enum_Socket_mode.NONE
            FrmCommunication_InitialSetting()
            result = ""
        Catch ex As Exception
            error_refresh(ex.ToString())
            result = ex.ToString()
        Finally

        End Try
        Return result
    End Function
    Public Sub Server_Stop()
        Try
            If Not TL_Server Is Nothing Then TL_Server.Stop() 'Accept 중지
            If Not thd_WaitClient Is Nothing Then thd_WaitClient.Abort() 'Accept 쓰레드 중지
            If Not Thd_PingTest Is Nothing Then Thd_PingTest.Abort() 'Pint_Test 쓰레드 중지
            If Not Thd_MainThread Is Nothing Then Thd_MainThread.Abort() 'Thread 관리 중지
            Del_Handler()
            Socket_CM_Resource_realease() '통신간 사용되는 Socket, 변수 리소스 초기화 및 해제
        Catch ex As Exception
            error_refresh(ex.ToString())
        End Try

    End Sub
    Public Sub Client_Stop()
        Try
            If Not TL_Server Is Nothing Then TL_Server.Stop() 'Accept 중지
            If Not thd_WaitClient Is Nothing Then thd_WaitClient.Abort() 'Accept 쓰레드 중지
            If Not Thd_PingTest Is Nothing Then Thd_PingTest.Abort() 'Pint_Test 쓰레드 중지
            If Not Thd_MainThread Is Nothing Then Thd_MainThread.Abort() 'Thread 관리 중지
            Del_Handler()
            Socket_CM_Resource_realease() '통신간 사용되는 Socket, 변수 리소스 초기화 및 해제
        Catch ex As Exception
            error_refresh(ex.ToString())
        End Try

    End Sub
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 명시적으로 호출되면 관리되지 않는 리소스를 해제합니다.
            End If
            Server_Stop()
            ' TODO: 관리되지 않는 공유 리소스를 해제합니다.
        End If
        Me.disposedValue = True
    End Sub
    Private Sub Dispose() Implements IDisposable.Dispose
        ' 이 코드는 변경하지 마십시오. 위의 Dispose(ByVal disposing As Boolean)에 정리 코드를 입력하십시오.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#Region "HashTable"
    Public Function CM_HashTable_List() As String()
        Dim str_Array() As String
        Try
            ReDim str_Array(CM_HashTable.Count - 1)
            CM_HashTable.Keys.CopyTo(str_Array, 0)
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally

        End Try
        Return str_Array
    End Function
    Private Function CM_HashTable_Search(ByVal Key_Value As Socket_Connection_Info, Optional ByVal Serach_Type As String = "Key") As Boolean
        Try
            If Serach_Type = "Key" Then
                If CM_HashTable.ContainsKey(Key_Value.ip & ":" & Key_Value.port) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                If CM_HashTable.ContainsValue(Key_Value.idx_socket_cm) = True Then
                    Return True
                Else
                    Return False
                End If
            End If

        Catch ex As Exception
            error_refresh(ex.ToString())
            Return False
        End Try
    End Function
    Private Function CM_HashTable_Search(ByVal Key_Value As IAsyncResult, Optional ByVal Serach_Type As String = "Key") As Boolean
        Try
            If Serach_Type = "Key" Then
                If CM_HashTable.ContainsKey(Key_Value.AsyncState.Split("|")(0)) = True Then
                    Return True
                Else
                    Return False
                End If
            Else
                If CM_HashTable.ContainsValue(Key_Value.AsyncState.Split("|")(1)) = True Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
            Return False
        End Try
    End Function
    Private Function CM_HashTable_Key_ReturnValue(ByVal Key_Value As Socket_Connection_Info) As Integer
        Try

            If CM_HashTable.ContainsKey(Key_Value.ip & ":" & Key_Value.port) = True Then
                Return CM_HashTable.Item(Key_Value.ip & ":" & Key_Value.port)
            Else
                Return -1
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
            Return -1
        End Try
    End Function
    Private Function CM_HashTable_add(ByVal Key_Value As Socket_Connection_Info) As Boolean
        Try
            If CM_HashTable_Search(Key_Value, "Key") = False Then CM_HashTable.Add(Key_Value.ip & ":" & Key_Value.port, Key_Value.idx_socket_cm) : Return True
        Catch ex As Exception
            error_refresh(ex.ToString())
            Return False
        End Try
    End Function
    Private Function CM_HashTable_add(ByVal Key_Value As IAsyncResult) As Boolean
        Try
            If CM_HashTable_Search(Key_Value.AsyncState, "Key") = False Then CM_HashTable.Add(Key_Value.AsyncState.Split("|")(0), Key_Value.AsyncState.Split("|")(1))

        Catch ex As Exception
            error_refresh(ex.ToString())
            Return False
        End Try
    End Function
    Private Function CM_HashTable_Delete(ByVal Key_Value As Socket_Connection_Info) As Boolean
        Try
            If CM_HashTable_Search(Key_Value) = True Then CM_HashTable.Remove(Key_Value.ip & ":" & Key_Value.port)
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function CM_HashTable_Delete(ByVal Key_Value As IAsyncResult) As Boolean
        Try
            If CM_HashTable_Search(Key_Value.AsyncState, "Key") = True Then CM_HashTable.Remove(Key_Value.AsyncState.split("|")(0))
        Catch ex As Exception
            error_refresh(ex.ToString())
            Return False
        End Try
    End Function
#End Region
#Region "Sock_Event"
    Public Sub Enqueue_Socket_Event()
        Dim socket_event As Socket_Event = New Socket_Event

    End Sub
    Private Sub Recive_Data(ByVal Async_Result As IAsyncResult, ByVal data_length As Integer)
        Dim str_RecieveData As String = ""
        Dim bytesRead As Integer = 0
        Dim connecion_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            connecion_info = CType(Async_Result.AsyncState, Socket_Connection_Info)
            'Console.WriteLine("RCV : " + DateTime.Now.ToString("ss.fff"))
            Dim socket_event As Socket_Event = New Socket_Event
            socket_event.status = Socket_Event_Type.Recieve
            socket_event.dt_occur = DateTime.Now
            socket_event.info_ip = connecion_info.ip
            socket_event.info_port = connecion_info.port
            socket_event.data_length = data_length
            socket_event.info_ip_port_type1 = socket_event.info_ip + ":" + socket_event.info_port.ToString()
            Array.Resize(Of Byte)(socket_event.data_to_array, data_length)
            Array.Copy(byte_DataBuffer(connecion_info.idx_socket_cm), socket_event.data_to_array, data_length)
            'socket_event.rcv_data_to_array =  byte_DataBuffer(Async_Result.AsyncState.split("|")(1))
            str_RecieveData = System.Text.Encoding.Default.GetString(byte_DataBuffer(connecion_info.idx_socket_cm), 0, data_length)
            socket_event.data_to_string = str_RecieveData
            'Console.WriteLine(socket_event.status.ToString() & " - " & DateTime.Now.ToString("ss.fff"))
            m_Col_Socket_Event_List.Enqueue(socket_event)

            If m_Col_Socket_Event_List.Count >= 100 Then m_Col_Socket_Event_List.TryDequeue(socket_event)


        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            Socket_CM(connecion_info.idx_socket_cm).BeginReceive(byte_DataBuffer(connecion_info.idx_socket_cm), SocketFlags.None, Int_bufferSize, SocketFlags.None, AddressOf ReceiveCallback, connecion_info)
        End Try
    End Sub
    Private Sub Send_Completed(ByVal connection_info As Socket_Connection_Info, ByVal Bt_DataByte() As Byte)
        Dim Sock_Num As Integer = -1
        Dim socket_event As Socket_Event = New Socket_Event
        'Dim BT_IniTemp(0) As Byte : BT_IniTemp(0) = Nothing
        Try
            '소켓이 접속되어있으면 바로 전송 / 접속이 안되어있으면 접속 이벤트 발생 후 메시지 전송
            If CM_HashTable_Search(connection_info, "Key") = True Then
                Sock_Num = CM_HashTable_Key_ReturnValue(connection_info)

                'If Not Sock_Num = -1 Then Socket_CM(Sock_Num).BeginSend(Bt_DataByte, 0, Bt_DataByte.Length, SocketFlags.None, New AsyncCallback(AddressOf SendCallback), Sender & "|" & Sock_Num)
                If Not Sock_Num = -1 Then
                    If Socket_CM(Sock_Num) Is Nothing Then Exit Sub
                    Socket_CM(Sock_Num).NoDelay = True
                    Socket_CM(Sock_Num).Send(Bt_DataByte, 0, Bt_DataByte.Length, SocketFlags.None)
                Else
                    If Socket_CM(Sock_Num) Is Nothing Then Exit Sub
                End If
                If use_queue_send_event = True Then
                    socket_event.info_ip = connection_info.ip
                    socket_event.info_port = connection_info.port
                    socket_event.info_ip_port_type1 = socket_event.info_ip + ":" + socket_event.info_port.ToString()
                    socket_event.status = Socket_Event_Type.Send
                    socket_event.dt_occur = DateTime.Now
                    socket_event.data_length = Bt_DataByte.Length
                    Array.Resize(Of Byte)(socket_event.data_to_array, Bt_DataByte.Length)
                    Array.Copy(Bt_DataByte, socket_event.data_to_array, Bt_DataByte.Length)
                    socket_event.data_to_string = System.Text.Encoding.Default.GetString(Bt_DataByte, 0, Bt_DataByte.Length)
                    m_Col_Socket_Event_List.Enqueue(socket_event)
                    If m_Col_Socket_Event_List.Count >= 100 Then m_Col_Socket_Event_List.TryDequeue(socket_event)
                End If
                dt_last_SendTime_Socket(Sock_Num) = Now
            Else
                RaiseEvent Ev_Thread_Connect(connection_info, "Server_To_Client", Bt_DataByte)
                'If Math.Abs(DateDiff(DateInterval.Second, dt_last_SendTime_Socket(Sock_Num), Now)) >= 2 Then
                '    m_ST_MainSensor.ST_Sensor_Setting(CInt(m_ST_MainSensor.target_Ip_Mapping.Item(Sender.ToString.Split("|")(0)))).bol_status_amp_ping = False
                'End If
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
            'AMP Send의 경우 2초이상 데이타를 보내지 못하면 네트워크 연결이 끊어져있다고 판단하고 Amp상태를 false로 바꾼다
            'If Math.Abs(DateDiff(DateInterval.Second, dt_last_SendTime_Socket(Sock_Num), Now)) >= 2 Then
            ' m_ST_MainSensor.ST_Sensor_Setting(CInt(m_ST_MainSensor.target_Ip_Mapping.Item(Sender.ToString.Split("|")(0)))).bol_status_amp_ping = False
            'IO.File.AppendAllText("C:\LOGFILE_" & Format(Now, "yyyyMM") & ".txt", Format(Now, "yyyyMMddhhmmss") & "NETWORK ERROR : (" & Sender.ToString & ")" & ex.Message & vbCrLf)
            'End If

            'Error_Display("Send Fail")
            '김원호
            'RaiseEvent Ev_Thread_Error(Sender & "|" & Sock_Num)
        End Try
    End Sub
    Private Sub SendCallback(ByVal Async_Result As IAsyncResult)
        'Monitor.Enter(Obj_Monitor_CallBack_Send)
        Dim connecion_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            connecion_info = CType(Async_Result.AsyncState, Socket_Connection_Info)
            Dim bytesSent As Integer = Socket_CM(connecion_info.idx_socket_cm).EndSend(Async_Result)
            '
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            'Monitor.Exit(Obj_Monitor_CallBack_Send)
        End Try
    End Sub
    Private Sub ConnectCallback(ByVal Async_Result As IAsyncResult)
        'Monitor.Enter(Obj_Monitor_CallBack_Connect)
        Dim str_arrBuffer(Int_bufferSize) As Byte
        Dim socket_event As Socket_Event = New Socket_Event
        Dim connection_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            connection_info = CType(Async_Result.AsyncState, Socket_Connection_Info)
            Socket_CM(connection_info.idx_socket_cm).EndConnect(Async_Result)
            Socket_CM(connection_info.idx_socket_cm).BeginReceive(byte_DataBuffer(connection_info.idx_socket_cm), 0, Int_bufferSize, 0, AddressOf ReceiveCallback, Async_Result.AsyncState)
            If Not connection_info.connect_and_send_data_to_byte Is Nothing AndAlso connection_info.connect_and_send_data_to_byte.Length = 0 Then

            Else
                Socket_CM(connection_info.idx_socket_cm).BeginSend(connection_info.connect_and_send_data_to_byte, 0, connection_info.connect_and_send_data_to_byte.Length, SocketFlags.None, New AsyncCallback(AddressOf SendCallback), connection_info)
                If use_queue_send_event = True Then
                    'Console.WriteLine("CALLBACK : " + DateTime.Now.ToString("ss.fff"))
                    socket_event.info_ip = connection_info.ip
                    socket_event.info_port = connection_info.port
                    socket_event.info_ip_port_type1 = socket_event.info_ip + ":" + socket_event.info_port.ToString()
                    socket_event.status = Socket_Event_Type.Send
                    socket_event.dt_occur = DateTime.Now
                    socket_event.data_length = connection_info.connect_and_send_data_to_byte.Length
                    Array.Resize(Of Byte)(socket_event.data_to_array, connection_info.connect_and_send_data_to_byte.Length)
                    Array.Copy(connection_info.connect_and_send_data_to_byte, socket_event.data_to_array, connection_info.connect_and_send_data_to_byte.Length)
                    socket_event.data_to_string = System.Text.Encoding.Default.GetString(connection_info.connect_and_send_data_to_byte, 0, connection_info.connect_and_send_data_to_byte.Length)
                    'If socket_event.status = Socket_Event_Type.None Then
                    '    Console.WriteLine("CallBack Status None : " + DateTime.Now.ToString("ss.fff"))
                    'End If
                    m_Col_Socket_Event_List.Enqueue(socket_event)
                    If m_Col_Socket_Event_List.Count >= 100 Then m_Col_Socket_Event_List.TryDequeue(socket_event)
                End If
            End If

        Catch ex As Exception
            error_refresh(ex.ToString())
            RaiseEvent Ev_Thread_Error(Async_Result)
        Finally
            'Monitor.Exit(Obj_Monitor_CallBack_Connect)
        End Try

    End Sub
    Private Sub ReceiveCallback(ByVal Async_Result As IAsyncResult)
        'Monitor.Enter(Obj_Monitor_CallBack_Recieve)
        'CheckForIllegalCrossThreadCalls = False
        Dim bytesRead As Integer = 0
        Dim connection_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            'Async_Result.AsyncState = Socket_Connection_Info type
            connection_info = CType(Async_Result.AsyncState, Socket_Connection_Info)
            If Not Socket_CM(connection_info.idx_socket_cm) Is Nothing Then
                bytesRead = Socket_CM(connection_info.idx_socket_cm).EndReceive(Async_Result)
                If bytesRead = 0 Then
                    RaiseEvent Ev_Thread_Disconnect_Async(Async_Result)
                Else
                    RaiseEvent Ev_Thread_RecieveCompleted(Async_Result, bytesRead)
                End If
            End If

        Catch ex As Exception
            error_refresh(ex.ToString())
            'Debug.Print("Recievecallback ERROR : " & " : " & Format(Now, "ss.fff"))
            RaiseEvent Ev_Thread_Disconnect_Async(Async_Result)
        Finally
            'Monitor.Exit(Obj_Monitor_CallBack_Recieve)
        End Try

    End Sub 'ReceiveCallback
    Private Sub Connection_Error(ByVal Async_Result As IAsyncResult)
        RaiseEvent Ev_Thread_Disconnect_Async(Async_Result)
    End Sub
    Private Sub Connection_Error(ByVal connection_info As Socket_Connection_Info)
        RaiseEvent Ev_Thread_Disconnect_Object(connection_info)
    End Sub
    Private Sub Connection_Open(ByVal connection_info As Socket_Connection_Info, ByVal str_Type As String, ByVal BT_InitialByte() As Byte)
        'CheckForIllegalCrossThreadCalls = False
        Dim Convert_Type_Adject(Int_bufferSize) As Byte
        Dim Sock_Num As Integer = 0
        Try
            'Monitor.Enter(Obj_Monitor_ConnectionOpen)
            If CM_HashTable_Search(connection_info, "Key") = True Then
                '키가 이미 등록되 있는 경우 -> 현재 Socket이 연결되 있다.
            Else
                If str_Type = "Client_To_Server" Then '소켓 접속 요청을 받음
                    byte_DataBuffer(connection_info.idx_socket_cm) = Convert_Type_Adject 'RecieveData Object -> Byte Type Alter
                    '비동기 Accept후 Data Recieve를 위한 BeginRecieve 비동기 실행
                    Socket_CM(connection_info.idx_socket_cm).BeginReceive(byte_DataBuffer(connection_info.idx_socket_cm), 0, Int_bufferSize, 0, AddressOf ReceiveCallback, connection_info) '
                    'IP, Port | Sock_Num 관리를 위한 HashTable 등록
                    CM_HashTable_add(connection_info) '127.0.0.1:5000|0
                    Debug.Print(connection_info.ip & ":" & connection_info.port)
                    Dim socket_event As Socket_Event = New Socket_Event
                    socket_event.status = Socket_Event_Type.Open
                    socket_event.dt_occur = DateTime.Now
                    socket_event.info_ip = connection_info.ip
                    socket_event.info_port = connection_info.port
                    socket_event.info_ip_port_type1 = socket_event.info_ip + ":" + socket_event.info_port.ToString()
                    m_Col_Socket_Event_List.Enqueue(socket_event)

                ElseIf str_Type = "Server_To_Client" Then '소켓 접속 요청을 한다. 호출전 HashTable 검사 후 키값이 없음을 확인함
                    If Col_Value.Count > 0 Then
                        Sock_Num = Col_Value.Item(1) : Col_Value.Remove(1) '사용한 Sock_Num 재사용을 막기위하여 제거
                        byte_DataBuffer(Sock_Num) = Convert_Type_Adject 'RecieveData Object -> Byte Type Alter

                        '접속한 IP에 SocketNo 할당 값 저장
                        connection_info.idx_socket_cm = Sock_Num
                        CM_HashTable_add(connection_info) '127.0.0.1:5000|0

                        Socket_CM(Sock_Num) = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        Array.Resize(Of Byte)(connection_info.connect_and_send_data_to_byte, BT_InitialByte.Length)
                        Array.Copy(byte_DataBuffer(connection_info.idx_socket_cm), connection_info.connect_and_send_data_to_byte, BT_InitialByte.Length)
                        connection_info.connect_and_send_data_to_byte = BT_InitialByte
                        connection_info.connect_and_send_data_to_string = System.Text.Encoding.Default.GetString(BT_InitialByte)
                        'Socket_CM(Sock_Num).BeginConnect(connection_info.ip, connection_info.port, New AsyncCallback(AddressOf ConnectCallback), Sender & "|" & Sock_Num & "|" & System.Text.Encoding.Default.GetString(BT_InitialByte))
                        Socket_CM(Sock_Num).BeginConnect(connection_info.ip, connection_info.port, New AsyncCallback(AddressOf ConnectCallback), connection_info)
                    End If

                End If
                'ListBox3.Items.Add(Sender.split("|")(0).split(":")(0) & ":" & Sender.split("|")(0).split(":")(1))

            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
            RaiseEvent Ev_Thread_Disconnect_Object(connection_info)
        Finally
            'Monitor.Exit(Obj_Monitor_ConnectionOpen)
        End Try
    End Sub
    Private Sub Connection_Close_Async(ByVal Async_Result As IAsyncResult) '통신간 에러 발생시 자동 소켓 중단
        '연결 중인 Socket 삭제
        Dim connection_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            connection_info = CType(Async_Result.AsyncState, Socket_Connection_Info)
            'Monitor.Enter(obj_Monitor_ConnectionClose)
            If Not Socket_CM(connection_info.idx_socket_cm) Is Nothing Then 'Socket이 사용된 경우
                If Socket_CM(connection_info.idx_socket_cm).Connected = True Then
                    Socket_CM(connection_info.idx_socket_cm).Shutdown(SocketShutdown.Both) 'Socket을 ShutDown하여 Recieve, Send 둘다 중지 시킨다. 
                End If

                Socket_CM(connection_info.idx_socket_cm).Close() 'Socket과 관련한 모든 리소스 해제
                Socket_CM(connection_info.idx_socket_cm) = Nothing 'Socket 초기화
            End If
            'Recieve Data 저장간 사용된 Object 변수 초기화
            If Not byte_DataBuffer(connection_info.idx_socket_cm) Is Nothing Then
                byte_DataBuffer(connection_info.idx_socket_cm) = Nothing
            End If
            'HashTable데이타 제거
            If CM_HashTable_Search(connection_info, "Key") = True Then 'Key = IP : Value = Sock_Number
                CM_HashTable_Delete(connection_info)
                Dim socket_event As Socket_Event = New Socket_Event
                socket_event.status = Socket_Event_Type.Close
                socket_event.dt_occur = DateTime.Now
                socket_event.info_ip = connection_info.ip
                socket_event.info_port = connection_info.port
                m_Col_Socket_Event_List.Enqueue(socket_event)
            End If
            'Return Using Sock_Num -> Re Using Sock_Num  / idx 재활용
            If Col_Value.Contains(connection_info.idx_socket_cm) = False Then
                Col_Value.Add(connection_info.idx_socket_cm)
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            'Monitor.Exit(obj_Monitor_ConnectionClose)
        End Try

    End Sub
    Private Sub Connection_Close_Object(ByVal connection_info As Socket_Connection_Info) '사용중 임의로 소켓 중단
        Try
            'Monitor.Enter(obj_Monitor_ConnectionClose)
            '연결 중인 Socket 삭제
            Dim Sock_Num As Integer = -1
            Sock_Num = CM_HashTable_Key_ReturnValue(connection_info)
            If Sock_Num = -1 Then Exit Sub
            '김원호
            'If str_SocketState = "" Or str_SocketState.Split("|").Length < 1 Then Exit Sub

            If Not Socket_CM(Sock_Num) Is Nothing Then 'Socket이 사용된 경우 
                If Socket_CM(Sock_Num).Connected = True Then
                    Socket_CM(Sock_Num).Shutdown(SocketShutdown.Both) 'Socket을 ShutDown하여 Recieve, Send 둘다 중지 시킨다. 
                End If
                Socket_CM(Sock_Num).Close() 'Socket과 관련한 모든 리소스 해제
                Socket_CM(Sock_Num) = Nothing 'Socket 초기화
            End If
            'Recieve Data 저장간 사용된 Object 변수 초기화
            If Not byte_DataBuffer(Sock_Num) Is Nothing Then
                byte_DataBuffer(Sock_Num) = Nothing
            End If
            If connection_info.req_clear_hashtableinfo_by_server = False Then
                'HashTable데이타 제거 // 서버에서 요청했을 경우 개별 삭제 안함 /  CM_Resoureces_remove에서 진행
                If CM_HashTable_Search(connection_info, "Key") = True Then 'Key = IP : Value = Sock_Number
                    CM_HashTable_Delete(connection_info)
                End If
            End If

            'Return Using Sock_Num -> Re Using Sock_Num
            If Col_Value.Contains(Sock_Num) = False Then
                Col_Value.Add(Sock_Num)
            End If
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            'Monitor.Exit(obj_Monitor_ConnectionClose)
        End Try


    End Sub
#End Region
#Region "Operating Thread"
    Public Sub Manage_Thread()
        'Monitor.Enter(Obj_Monitor_Thd_MainThread)
        Try
            While (1)

                If Not thd_WaitClient Is Nothing Then
                    If Not thd_WaitClient.ThreadState = ThreadState.Running Then
                        thd_WaitClient.Abort()
                        thd_WaitClient = Nothing
                        thd_WaitClient = New Threading.Thread(AddressOf StartListen)

                        thd_WaitClient.Start()
                    End If

                Else
                    thd_WaitClient = New Threading.Thread(AddressOf StartListen)
                    thd_WaitClient = Nothing
                    thd_WaitClient.Start()
                End If
                System.Threading.Thread.Sleep(10000) '10초마다 내부 스레드의 상태를 확인한다.
            End While
        Catch ex As AbandonedMutexException
            error_refresh(ex.ToString())
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            'Monitor.Exit(Obj_Monitor_Thd_MainThread)
        End Try
    End Sub

    Private Sub HashTable_PingTest()
        Dim str_Array() As String
        Try
            While (1)

                'Monitor.Enter(Obj_Monitor_Thd_PingTest)
                System.Threading.Thread.Sleep(Int_Interval_PingTest)

                ReDim str_Array(CM_HashTable.Count - 1) : CM_HashTable.Keys.CopyTo(str_Array, 0)

                'For i As Integer = 0 To UBound(str_Array)
                '    If My.Computer.Network.Ping(str_Array(i).Split(":")(0)) = False Then RaiseEvent Ev_Thread_Disconnect_Object(str_Array(i))
                'Next

                'Monitor.Exit(Obj_Monitor_Thd_PingTest)
            End While

        Catch ex As AbandonedMutexException
            error_refresh(ex.ToString())
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            'Monitor.Exit(Obj_Monitor_Thd_PingTest)
        End Try
    End Sub

    Private Sub StartListen() 'Server Mode :  Clinet Accept
        Dim str_IP As String = ""
        Dim Sock_Num As Integer = 0
        Dim BT_IniTemp(0) As Byte : BT_IniTemp(0) = Nothing
        Try
            'Monitor.Enter(Obj_Monitor_Thd_WaitClient)
            'TL_Server = New TcpListener(IPAddress.Parse("127.0.0.1"), Int_PotNum)
            If Not TL_Server Is Nothing Then
                TL_Server.Stop()
                TL_Server = Nothing
            End If
            '서버 시작
            TL_Server = New TcpListener(server_port)
            TL_Server.Start()

            While (1)
                If Col_Value.Count > 0 Then
                    Sock_Num = Col_Value.Item(1) : Col_Value.Remove(1) '사용한 Sock_Num 재사용을 막기위하여 제거
                    Socket_CM(Sock_Num) = TL_Server.AcceptSocket
                    Dim connection_info As New Socket_Connection_Info
                    If Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":").Length = 2 Then
                        connection_info.ip = Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":")(0)
                        connection_info.port = Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":")(1)

                        If Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":")(0) <> "10.10.10.1" Or Socket_CM(Sock_Num).LocalEndPoint.ToString.Split(":")(0) <> "10.10.10.1" Then
                            ctc_connected_ip = Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":")(0)
                            ctc_connected_port = Socket_CM(Sock_Num).RemoteEndPoint.ToString.Split(":")(1)
                        End If

                        connection_info.idx_socket_cm = Sock_Num
                        'str_IP = "" : str_IP = Socket_CM(Sock_Num).RemoteEndPoint.ToString 'Connect Request IP, Port Check
                        RaiseEvent Ev_Thread_Connect(connection_info, "Client_To_Server", BT_IniTemp)
                    End If


                Else
                    '서버 CM 연결수 초과
                End If
                System.Threading.Thread.Sleep(10)
            End While
        Catch ex As System.Threading.ThreadAbortException
            error_refresh(ex.ToString())
        Catch ex As Exception
            error_refresh(ex.ToString())
        Finally
            ' Monitor.Exit(Obj_Monitor_Thd_WaitClient)
        End Try
    End Sub
#End Region
#Region "Function"
    Private Sub Socket_CM_Resource_realease()
        Dim connection_info As Socket_Connection_Info = New Socket_Connection_Info
        Try
            'ReDim str_Array(CM_HashTable.Count - 1) : CM_HashTable.Keys.CopyTo(str_Array, 0)
            For Each item As DictionaryEntry In CM_HashTable
                connection_info.ip = item.Key.ToString.Split(":")(0)
                connection_info.port = item.Key.ToString.Split(":")(1)
                connection_info.idx_socket_cm = item.Value
                connection_info.req_clear_hashtableinfo_by_server = True
                Connection_Close_Object(connection_info)
            Next
            CM_HashTable.Clear()
        Catch ex As Exception
            error_refresh(ex.ToString())
        End Try

    End Sub
    Public Sub SendMsg(ByVal connection_info As Socket_Connection_Info, ByVal str_Data As String)
        'Monitor.Enter()
        Dim byteSend() As Byte
        Try
            byteSend = Encoding.ASCII.GetBytes(str_Data)
            RaiseEvent Ev_Thread_SendCompleted(connection_info, byteSend)

        Catch ex As Exception
            error_refresh(ex.ToString())
            Exit Sub
            'RaiseEvent Ev_Thread_Error(Sock_State)
        End Try
    End Sub
    Public Sub SendMsg(ByVal connection_info As Socket_Connection_Info, ByVal byteSend() As Byte)
        'Monitor.Enter()
        Try
            RaiseEvent Ev_Thread_SendCompleted(connection_info, byteSend)
            'Console.WriteLine(byteSend.Length)
        Catch ex As Exception
            error_refresh(ex.ToString())
            Exit Sub
            'RaiseEvent Ev_Thread_Error(Sock_State)
        End Try
    End Sub

    Public Sub error_refresh(ByVal value As String)

        Try
            value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff" & " : " & value)
            m_Col_Error_List.Enqueue(value)
            '10개 이상 누적 시 FIFO 관리 / 사용자가 별도 큐에서 계속 가야함
            If m_Col_Error_List.Count > 10 Then
                m_Col_Error_List.Dequeue()
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region
End Class
