using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    /// <summary>
    /// 调度器，进行事件分发
    /// </summary>
    class Dispatcher
    {
        // command对应消息的处理函数
        public delegate void EventHandler(MsgBase msg);

        // 用于消息分发的dict
        private static Dictionary<int, EventHandler> eventHandlerDict = new Dictionary<int, EventHandler>();

        public static void HandlerEvent(int command, MsgBase msg)
        {
            lock (eventHandlerDict)
            {
                if (eventHandlerDict.ContainsKey(command))
                {
                    eventHandlerDict[command](msg);
                }
            }
        }

        public static void RegisterHandler(int command, EventHandler handler)
        {
            lock (eventHandlerDict)
            {
                if (eventHandlerDict.ContainsKey(command))
                {
                    eventHandlerDict[command] += handler;
                }
                else
                {
                    eventHandlerDict[command] = handler;
                }
            }
        }

        public static void RemoveHandler(int command, EventHandler handler)
        {
            lock (eventHandlerDict)
            {
                if (eventHandlerDict.ContainsKey(command))
                {
                    eventHandlerDict[command] -= handler;
                    if (eventHandlerDict[command] == null)
                    {
                        eventHandlerDict.Remove(command);
                    }
                }
            }
        }
    }
}
