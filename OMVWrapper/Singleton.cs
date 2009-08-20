
namespace OpenSimBot.OMVWrapper.Utility
{

    public class Singleton<T> where T : new()
    {
        private static T m_instance;
        public static T GetInstance()
        {
            if (null == m_instance)
            {
                m_instance = new T();
            }

            return m_instance;
        }
    }
}