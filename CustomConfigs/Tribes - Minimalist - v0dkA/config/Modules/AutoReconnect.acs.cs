function AutoReconnect::onMissionEnd() {
    schedule("AutoReconnect();", 8);
}

function AutoReconnect(%address) {

    $ConnectedToServer = FALSE;

    setCursor(MainWindow, "Cur_Arrow.bmp");

    disconnect();
    Event::Trigger(eventDisconnected);

    deleteObject(ConsoleScheduler);
    newObject(ConsoleScheduler, SimConsoleScheduler);

    cursorOn(MainWindow);

    myConnect($Server::Address, $Server::JoinPassword);
}

function myConnect(%serverIp, %serverPw)
{
    $Server::Address = %serverIp;
    $Server::JoinPassword = %serverPw;
    connect(%serverIp);
}

Event::Attach( eventChangeMission, AutoReconnect::onMissionEnd );