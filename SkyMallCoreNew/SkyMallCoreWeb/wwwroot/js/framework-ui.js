$(function () {
    document.body.className = localStorage.getItem('config-skin');
    if ($("[data-toggle='tooltip']").length > 0) {
        $("[data-toggle='tooltip']").tooltip();
    }
});
$.reload = function () {
    location.reload();
    return false;
};
$.loading = function (bool, text) {
    var $loadingpage = top.$("#loadingPage");
    var $loadingtext = $loadingpage.find('.loading-content');
    if (bool) {
        $loadingpage.show();
    } else {
        if ($loadingtext.attr('istableloading') === undefined) {
            $loadingpage.hide();
        }
    }
    if (!!text) {
        $loadingtext.html(text);
    } else {
        $loadingtext.html("数据加载中，请稍后…");
    }
    $loadingtext.css("left", (top.$('body').width() - $loadingtext.width()) / 2 - 50);
    $loadingtext.css("top", (top.$('body').height() - $loadingtext.height()) / 2);
};
$.request = function (name) {
    var search = location.search.slice(1);
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] === name) {
            if (unescape(ar[1]) === 'undefined') {
                return "";
            } else {
                return unescape(ar[1]);
            }
        }
    }
    return "";
};
$.currentWindow = function () {
    //var iframeId = top.$("._iframe:visible").attr("id");
    //return top.frames[iframeId];
    return top;
};
$.browser = function () {
    var userAgent = navigator.userAgent;
    var isOpera = userAgent.indexOf("Opera") > -1;
    if (isOpera) {
        return "Opera"
    };
    if (userAgent.indexOf("Firefox") > -1) {
        return "FF";
    }
    if (userAgent.indexOf("Chrome") > -1) {
        if (window.navigator.webkitPersistentStorage.toString().indexOf('DeprecatedStorageQuota') > -1) {
            return "Chrome";
        } else {
            return "360";
        }
    }
    if (userAgent.indexOf("Safari") > -1) {
        return "Safari";
    }
    if (userAgent.indexOf("compatible") > -1 && userAgent.indexOf("MSIE") > -1 && !isOpera) {
        return "IE";
    };
};
$.download = function (url, data, method) {
    if (url && data) {
        data = typeof data === 'string' ? data : jQuery.param(data);
        var inputs = '';
        $.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        $('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    };
};
$.openWindow = function (url, target) {
    var a = document.createElement('a');
    a.style = "display:none;";
    a.href = url;
    if (target) {
        a.target = target;
    }
    document.body.appendChild(a);
    a.click();
};
$.modalOpen = function (options) {
    var defaults = {
        id: null,
        title: '系统窗口',
        width: "100px",
        height: "100px",
        url: '',
        shade: 0.3,
        btn: ['确认', '关闭'],
        btnclass: ['btn btn-primary', 'btn btn-danger'],
        callBack: null
    };
    var options = $.extend(defaults, options);
    var _width = top.$(window).width() > parseInt(options.width.replace('px', '')) ? options.width : top.$(window).width() + 'px';
    var _height = top.$(window).height() > parseInt(options.height.replace('px', '')) ? options.height : top.$(window).height() + 'px';
    top.layer.open({
        id: options.id,
        type: 2,
        shade: options.shade,
        title: options.title,
        fix: false,
        area: [_width, _height],
        content: options.url,
        btn: options.btn,
        btnclass: options.btnclass,
        yes: function () {
            options.callBack(options.id);
        }, cancel: function () {
            return true;
        }
    });
};
$.modalConfirm = function (content, callBack) {
    top.layer.confirm(content, {
        icon: "fa-exclamation-circle",
        title: "系统提示",
        btn: ['确认', '取消'],
        btnclass: ['btn btn-primary', 'btn btn-danger']
    }, function () {
        callBack(true);
    }, function () {
        callBack(false);
    });
};
$.modalAlert = function (content, type, callback) {
    var icon = "";
    if (type === 'success') {
        icon = "fa-check-circle";
    }
    if (type === 'error') {
        icon = "fa-times-circle";
    }
    if (type === 'warning') {
        icon = "fa-exclamation-circle";
    }
    top.layer.alert(content, {
        icon: icon,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
        yes: callback
    });
};


$.modalAlertReload = function (content, type, loadpage) {
    var icon = "";
    if (type === 'success') {
        icon = "fa-check-circle";
    }
    if (type === 'error') {
        icon = "fa-times-circle";
    }
    if (type === 'warning') {
        icon = "fa-exclamation-circle";
    }
    top.layer.alert(content, {
        icon: icon,
        title: "系统提示",
        btn: ['确认'],
        btnclass: ['btn btn-primary'],
        yes: function () {
            if (loadpage === undefined) {
                 location.reload();
            }
            else {
                location.href = loadpage;
            }
        }
    });
};

$.modalMsg = function (content, type, loadpage) {
    if (type !== undefined && type.length > 0) {
        var icon = "";
        if (type === 'success') {
            icon === "fa-check-circle";
            if (content === null || content === undefined) {
                content = "操作成功！";
            }
        }
        if (type === 'error') {
            icon = "fa-times-circle";
            if (content === null || content === undefined) {
                content = "操作有误！";
            }
        }
        if (type === 'warning') {
            icon = "fa-exclamation-circle";
        }

        if (top.layer.parents) {
            top.layer.msg(content, { icon: icon, time: 4000, shift: 5 });
            if (top.$(".layui-layer-msg").length > 0) {
                top.$(".layui-layer-msg").find('i.' + icon).parents('.layui-layer-msg').addClass('layui-layer-msg-' + type);
            }
        } else {
            if (type !== 'success') {
                layer.msg(content);
            }
        }
    } else {
        //console.log((top.layer.parents)
        if (top) {
            top.layer.msg(content);
        }
        else {
            layer.msg(content);
        }
    }
    //loadpage
    if (loadpage !== undefined && loadpage) {
        setTimeout(function () {
            location.href = loadpage;
        }, 700);
    }
};

$.modalClose = function () {
    var index = top.layer.getFrameIndex(window.name); //先得到当前iframe层的索引
    if (index === undefined) {
        index = 1;
    }
    var $IsdialogClose = top.$("#layui-layer" + index).find('.layui-layer-btn').find("#IsdialogClose");
    var IsClose = $IsdialogClose.is(":checked");
    //console.log("index:" + index + "      $IsdialogClose.length:" + $IsdialogClose.length + " IsClose:" + IsClose);
    if ($IsdialogClose.length === 0) {
        IsClose = true;
    }
    if (IsClose) {
        top.layer.close(index);
    } else {
      //  console.log("reload");
        location.reload();
    }
};

$.modalCloseAll = function () {
    top.layer.closeAll('dialog');
};

$.submitForm = function (options) {
    var defaults = {
        url: "",
        param: [],
        loading: "正在提交数据...",
        success: null,
        close: true
    };
    var options = $.extend(defaults, options);
    $.loading(true, options.loading);
    window.setTimeout(function () {
        if ($('[name=__RequestVerificationToken]').length > 0) {
            options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
        }
        $.ajax({
            url: options.url,
            data: options.param,
            type: "post",
            dataType: "json",
            success: function (data) {
                if (data.state === "success") {
                    options.success(data);
                    $.modalMsg(data.message, data.state);
                    if (options.close === true) {
                        $.modalClose();
                    }

                } else {
                    $.modalAlert(data.message, data.state);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $.loading(false);
                $.modalMsg(errorThrown, "error");
            },
            beforeSend: function () {
                $.loading(true, options.loading);
            },
            complete: function () {
                $.loading(false);
            }
        });
    }, 500);
};
$.deleteForm = function (options) {
    var defaults = {
        prompt: "注：您确定要删除该项数据吗？",
        url: "",
        param: [],
        loading: "正在删除数据...",
        success: null,
        close: true
    };
    options = $.extend(defaults, options);
    if ($('[name=__RequestVerificationToken]').length > 0) {
        options.param["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    $.modalConfirm(options.prompt, function (r) {
        if (r) {
            $.loading(true, options.loading);
            window.setTimeout(function () {
                $.ajax({
                    url: options.url,
                    data: options.param,
                    type: "post",
                    dataType: "json",
                    success: function (data) {
                        if (data.state === "success") {
                            options.success(data);
                            $.modalMsg(data.message, data.state);
                        } else {
                            console.log(data);
                            $.modalAlert(data.message, data.state);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.loading(false);
                        $.modalMsg(errorThrown, "error");
                    },
                    beforeSend: function () {
                        $.loading(true, options.loading);
                    },
                    complete: function () {
                        $.loading(false);
                        setTimeout(function () {
                            $.modalCloseAll();
                        }, 2000);
                    }
                });
            }, 500);
        }
    });

};
$.jsonWhere = function (data, action) {
    if (action === null) return;
    var reval = new Array();
    $(data).each(function (i, v) {
        if (action(v)) {
            reval.push(v);
        }
    })
    return reval;
};
$.fn.jqGridRowValue = function () {
    var $grid = $(this);
    var selectedRowIds = $grid.jqGrid("getGridParam", "selarrrow");
    if (selectedRowIds != "") {
        var json = [];
        var len = selectedRowIds.length;
        for (var i = 0; i < len; i++) {
            var rowData = $grid.jqGrid('getRowData', selectedRowIds[i]);
            json.push(rowData);
        }
        return json;
    } else {
        return $grid.jqGrid('getRowData', $grid.jqGrid('getGridParam', 'selrow'));
    }
};
$.fn.formValid = function () {
    return $(this).valid({
        errorPlacement: function (error, element) {
            element.parents('.formValue').addClass('has-error');
            element.parents('.has-error').find('i.error').remove();
            element.parents('.has-error').append('<i class="form-control-feedback fa fa-exclamation-circle error" data-placement="left" data-toggle="tooltip" title="' + error + '"></i>');
            if ($("[data-toggle='tooltip']").length > 0) {
                $("[data-toggle='tooltip']").tooltip();
            }
            if (element.parents('.input-group').hasClass('input-group')) {
                element.parents('.has-error').find('i.error').css('right', '33px')
            }
        },
        success: function (element) {
            element.parents('.has-error').find('i.error').remove();
            element.parent().removeClass('has-error');
        }
    });
};

$.fn.IsValid = function () {
    return $(this).valid({
        errorPlacement: function (error, element) {
            if (!element.attr("disabled") && element.hasClass("required")) {
                element.parents('.formValue').addClass('has-error');
                element.parents('.has-error').find('span.error').remove();
                element.parents('.has-error').append('<span class="error" >' + error + '</span>');
                if (element.parents('.input-group').hasClass('input-group')) {
                    element.parents('.has-error').find('i.error').css('right', '33px')
                }
            }
        },
        success: function (element) {
            element.parents('.has-error').find('span.error').remove();
            element.parent('.formValue').removeClass('has-error');
        }
    });
};

$.fn.formSerialize = function (formdate) {
    var element = $(this);
    if (!!formdate) {
        for (var key in formdate) {
            var $id = element.find('#' + key);
            var value = $.trim(formdate[key]).replace(/&nbsp;/g, '');
            var type = $id.attr('type');
            if ($id.hasClass("select2-hidden-accessible")) {
                type = "select";
            }
            switch (type) {
                case "checkbox":
                    if (value === "true") {
                        $id.attr("checked", 'checked');
                    } else {
                        $id.removeAttr("checked");
                    }
                    break;
                case "select":
                    $id.val(value).trigger("change");
                    break;
                default:
                    $id.val(value);
                    break;
            }
        };
        return false;
    }
    var postdata = {};
    element.find('input,select,textarea').each(function (r) {
        var $this = $(this);
        var id = $this.attr('id');
        var type = $this.attr('type');
        switch (type) {
            case "checkbox":
                postdata[id] = $this.is(":checked");
                break;
            default:
                var value = $this.val() === "" ? "&nbsp;" : $this.val();
                //console.log($this)
                if (!$.request("keyValue")) {
                    value = value.replace(/&nbsp;/g, '');
                }
                postdata[id] = value;
                break;
        }
    });
    if ($('[name=__RequestVerificationToken]').length > 0) {
        postdata["__RequestVerificationToken"] = $('[name=__RequestVerificationToken]').val();
    }
    return postdata;
};
$.fn.bindSelect = function (options) {
    var defaults = {
        id: "id",
        text: "text",
        selected: '',
        search: false,
        url: "",
        param: [],
        change: null
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    if (options.url != "") {
        $.ajax({
            url: options.url,
            data: options.param,
            dataType: "json",
            async: false,
            success: function (data) {
                var hasSelVal = false;
                $.each(data, function (i) {
                    var val = data[i][options.id];
                    var $option = $("<option></option>").val(val).html(data[i][options.text]);
                    if (options.selected && options.selected !== '') {
                        if (val === options.selected) {
                            $option.attr("selected", 'selected');
                            hasSelVal = true;
                        }
                    }
                    $element.append($option);
                });

                try {
                    $element.select2({
                        minimumResultsForSearch: options.search === true ? 0 : -1
                    });
                    $element.on("change", function (e) {
                        if (options.change != null) {
                            options.change(data[$(this).find("option:selected").index()]);
                        }
                        $("#select2-" + $element.attr('id') + "-container").html($(this).find("option:selected").text().replace(/　　/g, ''));
                    });
                } catch (e) { }
                if (options.callback != null) {
                    options.callback(data);
                }

                if (hasSelVal) {
                    $element.val(options.selected);
                    $element.trigger("change");
                    //console.log('trigger...change...');
                }
            }
        });
    } else {
        $element.select2({
            minimumResultsForSearch: -1
        });
    }
};
$.fn.authorizeButton = function () {
    var moduleId = $("#moduleId").val();
    var dataJson = top.clients.authorizeButton[moduleId];
    var $element = $(this);
    $element.find('a[authorize=yes]').attr('authorize', 'no');
    if (dataJson != undefined) {
        $.each(dataJson, function (i) {
            if (dataJson[i].EnabledMark) {
                $element.find("#" + dataJson[i].EnCode).attr('authorize', 'yes');
            }
        });
    }
    $element.find("[authorize=no]").parents('li').prev('.split').remove();
    $element.find("[authorize=no]").parents('li').remove();
    $element.find('[authorize=no]').remove();
}
$.fn.dataGrid = function (options) {
    var defaults = {
        datatype: "json",
        autowidth: true,
        rownumbers: true,
        shrinkToFit: false,
        gridview: true
    };
    var options = $.extend(defaults, options);
    var $element = $(this);
    options["onSelectRow"] = function (rowid) {
        var length = $(this).jqGrid("getGridParam", "selrow").length;
        var $operate = $(".operate");
        if (length > 0) {
            $operate.animate({ "left": 0 }, 200);
        } else {
            $operate.animate({ "left": '-100.1%' }, 200);
        }
        $operate.find('.close').click(function () {
            $operate.animate({ "left": '-100.1%' }, 200);
        })
    };
    $element.jqGrid(options);
};
$.fn.addScript = function (id, src) {
    var $element = $(this);
    var scriptFile = document.createElement('script');
    scriptFile.setAttribute("id", id);
    scriptFile.setAttribute("type", "text/javascript");
    scriptFile.setAttribute("src", src);
    $element.append(scriptFile);
};

$.fn.addStyle = function (href) {
    var $element = $(this);
    var linkFile = document.createElement('link');
    linkFile.setAttribute("rel", "stylesheet");
    linkFile.setAttribute("href", href);
    $element.append(linkFile);
};



//间隔循环 加载或处理数据
$.execTimer = function (interval, callback) {
    if (interval <= 0) {
        interval = 1000;
    }
    $.loading(true);
    //if (code === false) {//异常，停止请求
    //    $.loading(false);
    //    console.log("close timer .... ");
    //    return;
    //}
    callback();
    console.log(interval);
    setTimeout(function () {
        $.execTimer(interval, callback);
    }, interval);
    $.loading(false);
    //setTimeout(callback, interval);
    

};

