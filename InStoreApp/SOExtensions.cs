using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStoreApp
{
    public static class SOExtensions
    {
        public async static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            var retTask = await Task.WhenAny(task, Task.Delay(duration))
                                    .ConfigureAwait(false);

            if (retTask is Task<T>) return task.Result;

           // Debug.WriteLine("Timeout");
            return default(T);
        }
    }
}
