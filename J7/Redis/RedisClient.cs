using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Configuration;

namespace J7.Redis
{
    /// <summary>
    /// Redis帮助类，调用示例 
    /// using (var client = TaoBao.Tier.Common.RedisClientHelper.GetClient())
    /// {
    ///     client.HashContainsEntry(hashId, key);
    /// }
    /// </summary>
    public static class RedisClient
    {
        public static PooledRedisClientManager pcm;
        /// <summary>
        /// appSettings配置节点，Redis链接配置，如<add key="RedisConnection" value="192.168.1.18:6379" />
        /// </summary>
        public static readonly string redisConnection = ConfigurationManager.AppSettings["RedisConnection"];
        /// <summary>
        /// appSettings配置节点，Redis授权配置，如<add key="RedisConnectionAuth" value="fanhuan" />
        /// </summary>
        public static readonly string redisConnectionAuth = ConfigurationManager.AppSettings["RedisConnectionAuth"];

        private static PooledRedisClientManager GetDefaultManager(long db)
        {
            if (pcm != null)
            {
                return pcm;
            }

            pcm = new PooledRedisClientManager(new[] { redisConnection }
                , new[] { redisConnection },
                new RedisClientManagerConfig()
                {
                    MaxWritePoolSize = 100,
                    MaxReadPoolSize = 10000,
                    AutoStart = true
                }, db, 50, 2);
            pcm.ConnectTimeout = 1000;
            pcm.IdleTimeOutSecs = 20;
            // pcm.PoolTimeout = 2000;
            return pcm;
        }

        //创建短连接试试吧。。。。。。。
        //public static RedisClient Redis = new RedisClient("192.168.1.18",6379,System.Configuration.ConfigurationSettings.AppSettings["RedisConnectionAuth"]);

        public static IRedisClient GetReadClient(long db = 0)
        {
            IRedisClient client = GetDefaultManager(db).GetReadOnlyClient();
            client.Password = redisConnectionAuth;
            return client;
        }

        public static IRedisClient GetClient(long db = 0)
        {
            IRedisClient client = GetDefaultManager(db).GetClient();
            //IRedisNativeClient nativeClient = (RedisNativeClient)GetDefaultManager(db).GetClient();
            client.Password = redisConnectionAuth;

            return client;
        }

        public static void DisposeClient(long db = 0)
        {

            IRedisClient client = GetDefaultManager(db).GetClient();
            //RedisNativeClient client = new RedisNativeClient(db);
            client.Dispose();

            //DisposablePooledClient disposepool = new DisposablePooledClient(GetDefaultManager());
        }



        #region "帮助拓展"
        /// <summary>
        /// 获取Redis存储类型为string 的数据 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(string key) where T : class
        {
            using (var client = GetReadClient())
            {
                var isHave = client.ContainsKey(key);
                try
                {
                    if (isHave) return client.Get<T>(key);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    //client.Dispose();
                }
                return default(T);
            }
        }
        /// <summary>
        /// 获取Redis存储类型为string 的数据 ,如果不存在就用函数返回的值,自动加入缓存.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fun"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> fun, DateTime expiresAt) where T : class
        {
            var chacheData = Get<T>(key);
            if (chacheData != null) return chacheData;
            var saveData = fun();
            using (var client = GetClient())
            {
                client.Set<T>(key, saveData, expiresAt);
                //client.Dispose();
                return saveData;
            }
        }
        /// <summary>
        /// 判断Hash里面是否有某值
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExistHashEntry(string hashId, string key)
        {
            var result = false;
            using (var client = RedisClient.GetClient())
            {
                result = client.HashContainsEntry(hashId, key);
            }
            return result;
        }
        /// <summary>
        /// 移除哈希里面的某值
        /// </summary>
        /// <param name="hashId"></param>
        /// <param name="key"></param>
        public static void RemoveHashEntry(string hashId, string key)
        {
            using (var client = RedisClient.GetClient())
            {
                if (client.HashContainsEntry(hashId, key))
                {
                    client.RemoveEntryFromHash(hashId, key);
                }
            }
        }
        /// <summary>
        /// 移除key对应的Redis缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            using (var client = GetClient())
            {
                client.Remove(key);
                //client.Dispose();
            }
        }
        #endregion
    }
}
