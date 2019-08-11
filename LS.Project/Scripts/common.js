; (function (window,document,$) {
    var lscom = {};
    var trim = function (strings) {
        return (strings || "").replace(/^(\s|\u00A0)+|(\s|\u00A0)+$/g, "");
    }

    var getTopWinow= function () {
        var p = window;
        while (p != p.parent) {
            p = p.parent;
        }
        return p;
    }

    var errorHandle = function (status, message) {
        var top = getTopWinow();
        if (status === 401) {

            top.location.href = "/Login/Index";
        }
        else if (status == 402) {

            top.location.href = "/Error/NotAuthorize";
        }
        else if (status == 404) {
            top.location.href = "/Error/NotFound";
        }
        else if (status == 500) {
            top.location.href = "/Error/ServerInternal";
        }
        else if (status == 502) {
            top.location.href = "/Error/InvalidResponse";
        }
        else {
            top.location.href = "/Error/Other";
        }
    }
    //获取字符长度
    lscom.strlength = function (_str) {
        _str = trim(_str);  //去除字符串的左右两边空格
        var strlength = _str.length;
        if (!strlength) {  //如果字符串长度为零，返回零
            return 0;
        }
        var chinese = _str.match(/[\u4e00-\u9fa5]/g); //匹配中文，match返回包含中文的数组
        return strlength + (chinese ? chinese.length  : 0); //计算字符个数
    }

    lscom.Ajax = function (param) {

        var data = param.data || {};
        var method = param.method || "POST";
        var dataType = param.dataType || 'json';
        var url = param.url;
        var success = param.success || $.noop;
        var complete = param.complete || $.noop;
        $.ajax({
            cache: false,
            url: url,
            type: method,
            data: data,
            dataType: dataType,
            success: success,
            error: function (jqXhr, textStatus, xhr) {
                errorHandle(jqXhr.status, xhr.statusText);
            },
            complete: function () {
                complete();
            }
        })
    }

    var ztreeCheckBox = function (opt) {
        var defaultOption = {
            treeControl: "",//树形菜单
            menuContent: "menuContent",
            displayControl:"",
            Nodes: [],
            triggerControl:""
        };
        this.setting= {
            check: {
                enable: true,
                    chkboxType: { "Y": "", "N": "" }
            },
            view: {
                dblClickExpand: false
            },
            data: {
                simpleData: {
                    enable: true
                }
            },
            callback: {
                beforeClick: this.beforeClick,
                onCheck: opt.onCheck
            }
        }
        this.options = $.extend({}, this.defaultOption, opt)
    }
    ztreeCheckBox.prototype = {
        beforeClick: function (treeId, treeNode) {
            var zTree = $.fn.zTree.getZTreeObj(this.options.treeId);
            zTree.checkNode(treeNode, !treeNode.checked, null, true);
            return false;
        },
        //onCheck: function (e, treeId, treeNode) {
        //    var zTree = $.fn.zTree.getZTreeObj(this.options.treeControl),
        //        nodes = zTree.getCheckedNodes(true),
        //        v = "";
        //    for (var i = 0, l = nodes.length; i < l; i++) {
        //        v += nodes[i].name + ",";
        //    }
        //    if (v.length > 0) v = v.substring(0, v.length - 1);
        //    var controlObj = $("#Rolecodes");
        //    controlObj.attr("value", v);
        //},
        showMenu: function () {
            var cObj = $("#"+this.options.displayControl);
            var cOffset = $("#" + this.options.displayControl).offset();
            var _self = this;
            $("#" + this.options.menuContent).css({ left: cOffset.left + "px", top: cOffset.top + cObj.outerHeight() + "px" }).slideDown("fast");
            $("body").on("mousedown", function () {
                if (!(event.target.id == _self.options.displayControl || event.target.id == _self.options.menuContent || $(event.target).parents("#" + _self.options.menuContent).length > 0)) {
                    _self.hideMenu();
                }
                });
        },
        hideMenu:function () {
            $("#"+ this.options.menuContent).fadeOut("fast");
            var _self = this;
            $("body").off("mousedown", function () {
                if (!(event.target.id == _self.options.displayControl || event.target.id == _self.options.menuContent || $(event.target).parents("#" + _self.options.menuContent).length > 0)) {
                    _self.hideMenu();
                }
            });
        },
       
        initTree: function () {
            $.fn.zTree.init($("#" + this.options.treeControl), this.setting, this.options.Nodes);
            var _self = this;
            $("#" + this.options.triggerControl).on("click", function () {
                _self.showMenu();
            });
        }
     
    }
    lscom.TreeCheckBox = function (options) {
       
        var ztreeckb = new ztreeCheckBox(options);

        ztreeckb.initTree();
       
    }
  
    $(document).keydown(function (e) {
        var ev = e || window.event;//获取event对象     
        var obj = ev.target || ev.srcElement;//获取事件源     
        var t = obj.type || obj.getAttribute('type');//获取事件源类型    

        //获取作为判断条件的事件类型  
        var vReadOnly = obj.getAttribute('readonly');
        var vEnabled = obj.getAttribute('enabled');
        //处理null值情况  
        vReadOnly = (vReadOnly == null) ? false : vReadOnly;
        vEnabled = (vEnabled == null) ? true : vEnabled;

        //当敲Backspace键时，事件源类型为密码或单行、多行文本的，  
        //并且readonly属性为true或enabled属性为false的，则退格键失效  
        var flag1 = (ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea") && (vReadOnly == true || vEnabled != true)) ? true : false;

        //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效  
        var flag2 = (ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea") ? true : false;

        //判断  
        if (flag2 || flag1) return false;
    });

    lscom.successCallback = function (layer, data, isreload) {
        if (data.code == 0) {
            layer.msg(data.msg, {
                icon: 1
            },
                function () {
                    //关闭当前frame
                    xadmin.close();
                    if (isreload) {
                        xadmin.father_reload();
                    }
                });
        }
        else {
            layer.msg(data.msg, {
                icon: 2
            });
        }
    }
    lscom.successSelfCallback = function (layer, data, fn) {
        if (data.code == 0) {
            layer.msg(data.msg, {
                icon: 1
            },
                function () {
                    if (fn != undefined && typeof fn == "function") {
                        fn();
                    }
                });
        }
        else {
            layer.msg(data.msg, {
                icon: 2
            });
        }
    }
    //外部调用方法
    window.lscom = lscom;

})(window, document, jQuery);

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)
// 例子：
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
// (new Date()).Format("yyyy-M-d h:m:s.S")   ==> 2006-7-2 8:9:4.18
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份
        "d+": this.getDate(), //日
        "h+": this.getHours(), //小时
        "m+": this.getMinutes(), //分
        "s+": this.getSeconds(), //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

document.oncontextmenu = function () {
    event.returnValue = false;
}
document.oncontextmenu = function () {
    return false;
}