//A framework for MessageSystem architecture style
//yunhai 2022/6/22
//MIT license
using System;
using System.Collections.Generic;
using System.Reflection;
using YH;

namespace YH 
{

	/// <summary>
	/// framework of a goup of message infomation
	/// </summary>
	public class Event<E> where E: Enum
	{
		public E type;
		//other info:

		public Event(E type)
		{
			this.type = type;
		}
	}

	/// <summary>
	/// Subscriber need to response a info
	/// </summary>
	public interface ISubscriber<E> where E: Enum
	{
		//News can add some details, maybe you dont need it most of time
		void Response(Event<E> news);
	}
	public partial class EventManager<E> where E: Enum
	{
		private  Dictionary<E, HashSet<ISubscriber<E>>> topicUserTable;//枚举哈希表是我封装过的工具，可以自行替换
		private  Queue<Event<E>> topics;//message queue

		public EventManager()
		{
			topics=new Queue<Event<E>>();
			
			FieldInfo[] fields = typeof(E).GetFields();
			topicUserTable=new Dictionary<E, HashSet<ISubscriber<E>>>(fields.Length);
			for(int i = 1; i < fields.Length; i++)//fields[0] 不是枚举的字段，感兴趣的话可以自己测试一下
			{
				topicUserTable.Add((E)fields[i].GetValue(null),new HashSet<ISubscriber<E>>());
				//Debug.log(fields[i] + " : " + (int)fields[i].GetValue(null));
			}
			
		}

		//to collect a news just happened
		/// <summary>
		/// 采集新闻,是记者上交新闻的渠道
		/// </summary>
		/// <param name="eventHappened"></param>
		public  void GatherEvent(E eventHappened)
		{
			var details=new Event<E>(eventHappened);
			GatherEvent(details);
		}

		public  void GatherEvent(Event<E> aNews)
		{
			topics.Enqueue(aNews);
			
			Notify();//即刻通知版本，也可选择外部控制通知时机
		}

		//for subscriber to decide what event are interesting for them
		public  void Subscribe(E news, ISubscriber<E> subscriber)
		{
			topicUserTable[news].Add(subscriber);
		}
		public  void UnSubscribe(E news, ISubscriber<E> subscriber)
		{
			topicUserTable[news].Remove(subscriber);
		}

		/// <summary>
		/// to notify topics to subscribers, and clear message queue
		/// 此框架实现的策略是一次简单的通知所有订阅者，并清空消息队列，可以自行更改此方法
		/// </summary>
		public  void Notify()
		{
			while (topics.Count>0)
			{
				Event<E> news = topics.Dequeue();
				foreach (var user in topicUserTable[news.type])
				{
					user.Response(news);
				}
			}
		}

	}

}



