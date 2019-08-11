using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Cores
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class PageCriteria
    {
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        private string _Fileds = "*";
        public string Fields
        {
            get { return _Fileds; }
            set { _Fileds = value; }
        }
        private string _PrimaryKey = "ID";
        public string PrimaryKey
        {
            get { return _PrimaryKey; }
            set { _PrimaryKey = value; }
        }
        private int _PageSize = 10;
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }
        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
        private string _Sort = string.Empty;
        public string Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        private string _Condition = string.Empty;
        public string Condition
        {
            get { return _Condition; }
            set { _Condition = value; }
        }
        private int _RecordCount;
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; }
        }
    }
    /// <summary>
    /// 返回分页数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageDataView<T>
    {
        private int _TotalNum;
        public PageDataView()
        {
            this._Items = new List<T>();
        }
        public int TotalNum
        {
            get { return _TotalNum; }
            set { _TotalNum = value; }
        }
        private IList<T> _Items;
        public IList<T> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        public int CurrentPage { get; set; }
        public int TotalPageCount { get; set; }
    }
}
