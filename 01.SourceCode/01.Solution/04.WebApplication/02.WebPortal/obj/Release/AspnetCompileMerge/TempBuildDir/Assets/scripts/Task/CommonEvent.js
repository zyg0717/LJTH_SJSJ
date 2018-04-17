"use strit";
    var Event = {};
    Event.params = {
        range: 50,//距下边界长度/单位px  
        elemt: 500,          //插入元素高度/单位px 
        maxnum: 2,          //设置加载最多次数  
        num: 0,
        totalheight: 0,
        done: false
    };
    Event.main = $(".tasklist");//主体元素                  
    Event.loadingBtn = $(".more");
    Event.timmer = null;
    Event.circleTimmer = null;
    
    Event.pageIndex = 1;

    Event.dateFormat = function (date) {
        var dateTime = new Date(date);
        var Y = dateTime.getFullYear(),
            M = dateTime.getMonth() + 1,
            D = dateTime.getDate(),
            H = dateTime.getHours(),
            Min = dateTime.getMinutes(),
            S = dateTime.getSeconds();
        var currentDate = Y + '-' + Event.numFormat(M) + '-' + Event.numFormat(D) + ' ' + Event.numFormat(H) + ":" + Event.numFormat(Min) + ":" + Event.numFormat(S);
        return currentDate;
    };

    Event.numFormat = function (num) {
        return num > 9 ? num : "0" + num;
    }
    
    Event.dataScroll = function (pageClass, circleClass, totalData) {
            var srollPos = $(window).scrollTop(); //滚动条距顶部距离(页面超出窗口的高度)  
            totalheight = parseFloat($(window).height()) + parseFloat(srollPos);
            if (($(document).height() - range) < totalheight && obj.taskList.length < 10) {
                Event.timmer = setTimeout(function () {
                    obj.loadMore();
                }, 1000);
                Event.circleTimmer = setTimeout(function () {
                    Event.circleProcessing(circleClass);
                }, 100);
                
            } else if (!$("#pagination").html()) {
                clearTimeout(Event.timmer);
                clearTimeout(Event.circleTimmer);
                Event.paginationMethod(pageClass,totalData, 10);
            } else {
                return;
            }
    };

    Event.circleProcessing = function (className) {
        var c = $("." + className);
        animateEle();
        $(window).scroll(function () {
            animateEle()
        });

        function animateEle() {
            var b = {
                top: $(window).scrollTop(),
                bottom: $(window).scrollTop() + $(window).height()
            };
            c.each(function () {
                if (b.top <= $(this).offset().top && b.bottom >= $(this).offset().top && !$(this).data('bPlay')) {
                    $(this).data('bPlay', true);
                    var a = $(this).find('span').text().replace(/\%/, '');
                    if ($(this).find("span").text() !== "0%") {
                        $(this).svgCircle({
                            parent: $(this)[0],
                            w: 80,
                            R: 34,
                            sW: 6,
                            color: ["#5cb85c", "#5cb85c", "#5cb85c"],
                            perent: [100, a],
                            speed: 150,
                            delay: 400
                        })
                    }
                    if ($(this).find("span").text() == "0%") {
                        $(this).find("span").css("color", "#a9a9a9");
                        $(this).svgCircle({
                            parent: $(this)[0],
                            w: 80,
                            R: 34,
                            sW: 6,
                            color: ["#c4cdda", "#c4cdda", "#c4cdda"],
                            perent: [100, a],
                            speed: 150,
                            delay: 400
                        })
                    }
                }
            })
        }
    };

    Event.paginationMethod = function (className, totalData) {

        $("." + className).pagination({
            totalData: totalData,
            jump: true,
            coping: true,
            current: 1,
            count: 2,
            showData: 10,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            callback: Event.pageCallback
        });

    }

    Event.pageCallback = function (data) {
        Event.pageIndex = data.getCurrent();
         vm.currentIndex = data.getCurrent();
        utils.log(vm.currentIndex);
        vm.getTasks(vm.currentIndex);
    }

    function paginationMethod(className, totalData) {

        $("." + className).pagination({
            totalData: totalData,
            jump: true,
            coping: true,
            current: 1,
            count: 2,
            showData: 10,
            homePage: '首页',
            endPage: '末页',
            prevContent: '上页',
            nextContent: '下页',
            callback: pageCallback
        });

    }
    function pageCallback(data) {
        vm.currentIndex = data.getCurrent();
        Event.pageIndex = data.getCurrent();
        utils.log(vm.currentIndex);
        vm.getTasks(vm.currentIndex);
    }

    var vm = new Vue({
        el: "#vue",
        data: {
            taskList: [],
            message: "加载更多",
            loadingMore: false,
            visible: true,
            keyWord: '',
            listNull: false,
            pageIndex: Event.pageIndex,
            currentIndex: '',
            totalCount: ''
        },
        methods: {
            getTasks: function (index) {
                Event.ajax({
                    'url': api_url + "api/tasks/query",
                    'data': {
                        "TaskTitle": "",
                        "TaskCreatorLoginName": "",
                        "IsOnlySelf": true,
                        "TimeRange": 1,
                        "TaskType": 1,
                        "PageIndex": index + 2,
                        "PageSize": 5
                    },
                    successLoad: function (res) {
                        this.taskList = [];
                        utils.log(res);
                        $.each(res.Data, function (index, item) {
                            if (item.TaskUserNodeCount != 0 && item.CompleteUserNodeCount != 0) {
                                item.ProcessCount = (item.TaskUserNodeCount / item.CompleteUserNodeCount) * 100;
                            } else {
                                item.ProcessCount = 0;
                            }
                            item.TaskCreateDateFormat = Event.dataFormat(item.TaskCreateDate);
                            this.taskList.push(item);
                        });

                    }
                });
            },
            conditonFilter: function () {
                var newTaskList = [];
                if (this.keyWord.length === 0) {
                    getTaskList();
                    circleTimmer = setTimeout(function () {
                        Event.circleProcessing("processingbar");
                    }, 100);
                } else {
                    for (var i = 0; i < this.taskList.length; i++) {
                        if (this.taskList[i].TaskTemplateName.indexOf(this.keyWord) >= 0) {
                            newTaskList.push(this.taskList[i]);
                        }
                    }
                    this.taskList = newTaskList;
                }
                return this.taskList;
            },
            dataScroll: function () {
                clearTimeout(Event.timmer);
                clearTimeout(Event.circleTimmer);
                var srollPos = $(window).scrollTop(); //滚动条距顶部距离(页面超出窗口的高度)  
                Event.params.totalheight = parseFloat($(window).height()) + parseFloat(srollPos);
                if (($(document).height() - Event.params.range) < Event.params.totalheight &&  vm.taskList.length < 10) {
                    Event.timmer = setTimeout(function () {
                        vm.loadMore();
                    }, 100);

                } else if (!$("#pagination").html()) {
                    clearTimeout(Event.timmer);
                    clearTimeout(Event.circleTimmer);
                    Event.paginationMethod("M-box", 18);
                } else {
                    return;
                }
            },
            loadMore: function () {
                Event.ajax({
                    'url': api_url + "api/tasks/query",
                    'data': {
                        "TaskTitle": "",
                        "TaskCreatorLoginName": "",
                        "IsOnlySelf": true,
                        "TimeRange": 1,
                        "TaskType": 1,
                        "PageIndex": vm.currentIndex + 1,
                        "PageSize": 5
                    },
                    successLoad: function (res) {
                        $.each(res.Data, function (index, item) {
                            if (item.TaskUserNodeCount != 0 && item.CompleteUserNodeCount != 0) {
                                item.ProcessCount = (item.TaskUserNodeCount / item.CompleteUserNodeCount) * 100;
                            } else {
                                item.ProcessCount = 0;
                            }
                            item.TaskCreateDateFormat = Event.dataFormat(item.TaskCreateDate);
                            vm.taskList.push(item);
                        });

                    }
                });
            }
        }
    });

    Event.ajax = function (options) {
        var params = {
            'method': 'POST',
            'url': '',
            'contentType': "application/json",
            'dataType': 'json',
            'data': {},
            'successLoad': function (result) {

            },
            'error': ''
        };
        var settings = $.extend(params,options);
        $.ajax({
            method: settings.method,
            data: settings.data,
            url: settings.url,
            success: function (respons, status) {
                if (status == "success") {
                    settings.successLoad(respons.Result);
                }
            },
            error: function (respons, textStatus) {
                if (textStatus == 409) {
                    alert(textStatus.ErrorMessage);
                }                
            }
        });
    };