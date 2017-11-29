namespace TradFriLibrary.Models
{
    //... more values may be found according to https://github.com/ggravlingen/pytradfri/blob/master/pytradfri/const.py

    public enum TradFriConstRoot
    {
        Devices = 15001,
        Groups = 15004,
        Moods = 15005,
        Switch = 15009,
        SmartTasks = 15010,
        Gateway = 15011,
        StartAction = 15013
    }

    public enum TradFriConstAttr
    {
        Auth = 9063,
        Psk = 9091,
        Identity = 9090,

        ApplicationType = 5750,

        CreatedAt = 9002,

        CommissioningMode = 9061,

        CurrentTimeUnix = 9061,
        CurrentTimeISO8601 = 9060,

        DeviceInfo = 3,

        GatewayTimeSource = 9071,
        GatewayUpdateProgress = 9055,

        HomekitID = 9083,

        ID = 9003,
        LastSeen = 9020,
        LightControl = 3311,

        Name = 9001,
        NTP = 9023,
        FirmwareVersion = 9029,
        FirstSetup = 9069,

        GatewayInfo = 15012,
        GatewayID = 9081,
        GatewayReboot = 9030,
        GatewayFactoryDefaults = 9031, //gw to factory defaults
        GatewayFactoryDefaultsMinMaxMSR = 5605,

        LightState = 5850, // 0 or 1
        LightDimmer = 5851, // Dimmer, not following spec: 0..255
        LightColorHex = 5706, // string representing a value in hex
        LightColorX = 5709,
        LightColorY = 5710,
        LightColorSaturation = 5707, //guess
        LightColorHue = 5708, //guess
        LightMireds = 5711,

        MasterTokenTag = 9036,

        OtaType = 9066,
        OtaUpdateState = 9054,
        OtaUpdate = 9037,

        ReachableState = 9019,

        RepeatDays = 9041,

        Sensor = 3300,
        SensorMinRangeValue = 5603,
        SensorMaxRangeValue = 5604,
        SensorMinMeasuredValue = 5601,
        SensorMaxMeasuredValue = 5602,
        SensorType = 5751,
        SensorUnit = 5701,
        SensorValue = 5700,

        StartAction = 9042, // array

        SmartTaskType = 9040, // 4 = transition | 1 = not home | 2 = on/off
        SmartTaskNotAtHome = 1,
        SmartTaskLightsOff = 2,
        SmartTaskWakeUp = 4,

        SmartTaskTriggerTimeInterval = 9044,
        SmartTaskTriggerTimeStartHour = 9046,
        SmartTaskTriggerTimeStartMin = 9047,

        SwitchPlug = 3312,
        SwitchPowerFactor = 5820,

        TransiotionTime = 5712

    }

    public enum TradFriConstMireds
    {
        Min = 40,
        Max = 600,

        MinWS = 250,
        MaxWS = 454
    }

    public enum TradFriSupport
    {
        Brightness = 1,
        ColorTemp = 2,
        HexColor = 4,
        RGBColor = 8,
        XYColor = 16
    }

    public static class TradFriColors
    {
        const string Blue = "4a418a";
        const string LightBlue = "6c83ba";
        const string SaturatedPurple = "8f2686";
        const string Lime = "a9d62b";
        const string LightPurple = "c984bb";
        const string Yellow = "d6e44b";
        const string SaturatedPink = "d9337c";
        const string DarkPeach = "da5d41";
        const string SaturatedRed = "dc4b31";
        const string ColdSky = "dcf0f8";
        const string Pink = "e491af";
        const string Peach = "e57345";
        const string WarmAmber = "e78834";
        const string LightPink = "e8bedd";
        const string CoolDaylight = "eaf6fb";
        const string CandleLight = "ebb63e";
        const string WarmGlow = "efd275";
        const string WarmWhite = "f1e0b5";
        const string Sunrise = "f2eccf";
        const string CoolWhite = "f5faf6";
    }

    public enum Bool
    {
        True = 1,
        False = 0
    }
}
