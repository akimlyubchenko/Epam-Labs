using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacherServices
{
    public interface ICacherServices
    {
        void SetCache(object obj, double timeLive, string tag);

        object GetCache(string tag);

        void ChangeTime(string tag, double newTimeLive);

        object RemoveCache(string tag);
    }
}
