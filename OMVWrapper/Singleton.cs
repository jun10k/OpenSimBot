
namespace OpenSimBot.OMVWrapper.Utility
{

    class Singleton<T>
    {
        private static const T m_instance = new T();
        public static const T& GetInstance()
        {
            return m_instance;

        }

    }




}