namespace ArRetarget
{
    //init method for handlers
    public interface IInit
    {
        void Init();
    }

    //used if data can be received
    public interface IGet<T>
    {
        void GetFrameData(T frame);
    }

    //interface for json string
    public interface IJson
    {
        string GetJsonString();
    }

    //interface for json prefix
    public interface IPrefix
    {
        string GetJsonPrefix();
    }

    //used if data gets pushed
    public interface IStop
    {
        void StopTracking();
    }
}
