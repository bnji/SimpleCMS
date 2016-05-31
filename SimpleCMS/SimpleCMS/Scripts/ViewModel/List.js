var indexVM;
var ViewModel = function (url, _pageCount, _pageSize) {//, _pageNumber, _pageCount) {
    var self = this;
    self._baseUrl = "#/page/";
    self._filterUrl = "/:filter:";
    self._sortUrl = "/:sort:";
    self._sortDirUrl = "/:dir:";
    self.app = $.sammy('#main', function () {
        this.get(self._baseUrl + ":number", function (context) {
            var pageNumber = this.params['number'];
            self.load(pageNumber);
        });
        this.get(self._baseUrl + ":number" + self._filterUrl + ":filter", function (context) {
            var pageNumber = this.params['number'];
            var filter = this.params['filter'];
            self.load(pageNumber, filter);
        });
        this.get(self._baseUrl + ":number" + self._sortUrl + ":sort" + self._sortDirUrl + ":dir", function (context) {
            var pageNumber = this.params['number'];
            var sortName = this.params['sort'];
            var sortDir = this.params['dir'];
            self.load(pageNumber, "", sortName, sortDir);
        });
        this.get(self._baseUrl + ":number" + self._filterUrl + ":filter" + self._sortUrl + ":sort" + self._sortDirUrl + ":dir", function (context) {
            var pageNumber = this.params['number'];
            var filter = this.params['filter'];
            var sortName = this.params['sort'];
            var sortDir = this.params['dir'];
            self.load(pageNumber, filter, sortName, sortDir);
        });
    });
    // All elements: The complete list of elements from which the current page will be sliced out.
    self.List = ko.observableArray([]);
    self.currentPage = ko.observable(1);
    self.pageSize = ko.observable(_pageSize);
    self.currentFilter = ko.observable("");
    self.currentSort = ko.observable("");
    self.pageCount = ko.observable(_pageCount > 0 ? _pageCount : 1);
    self.navigateTo = function (page, filter, sortName, sortType) {
        page = parseInt(page);
        if (page > 0 && page < self.pageCount() + 1) {
            if (self._onNavigate) {
                self._onNavigate();
            }
            var url = self._baseUrl + page;
            if (filter && filter.length > 0) {
                url += self._filterUrl + filter;
            }
            if (sortName && sortName.length > 0) {
                url += self._sortUrl + sortName;
            }
            if (sortType && sortType.length > 0) {
                url += self._sortDirUrl + sortType;
            }
            window.location = url;
            //filter = filter !== undefined || filter.length > 0 ? "/" + filter : "";
            //alert(filter);
        }
    };
    self.back = function () {
        var page = parseInt(self.currentPage()) - 1;
        self.navigateTo(page);
    };
    self.forward = function () {
        var page = parseInt(self.currentPage()) + 1;
        self.navigateTo(page);
    };
    self._itemCount = ko.observable(0);
    self.loadNextPage = function () {
        //self.load(self.currentPage());
        //self._itemCount(self._itemCount() + 1);
        //console.log("items on page: " + self._itemCount());
        //console.log(self.List().length);
        console.log(_.lastIndexOf(self.List(), _.last(self.List())) + 1);
        if (_.lastIndexOf(self.List(), _.last(self.List())) + 1 === 3) return;

        self._itemCount(_.lastIndexOf(self.List(), _.last(self.List())) + 1);
        console.log("itemCount: " + self._itemCount());
        if (self._itemCount() === 3) {// === self.List().length) {
            // Increase page count
            self.pageCount(self.pageCount() + 1);
            var page = self.pageCount();
            console.log(page);
            if (self._onLoadNextPage) {
                self._onLoadNextPage();
            }
            self.navigateTo(page);
            //console.log("loading page " + nextPage);
            //indexVM.load(2);
        }
        else {
            self.load(self.currentPage());
        }
    };
    self._onNavigate = undefined;
    self._onLoadNextPage = undefined;
    self.onNavigate = function (callback) {
        if (callback && !self._onNavigate)
            self._onNavigate = callback;
    };
    self.onLoadNextPage = function (callback) {
        if (callback && !self._onLoadNextPage)
            self._onLoadNextPage = callback;
    };
    self.filter = function (filter) {
        self.navigateTo(self.currentPage(), filter);
        //self.load(self.currentPage(), filter);
    };
    self.reload = function () {
        self.navigateTo(self.currentPage(), "");
    };
    self.currentSortType = ko.observable("asc");
    self.sort = function (sortName) {
        if (self.currentSortType() === "asc") {
            self.currentSortType("desc");
        }
        else {
            self.currentSortType("asc");
        }
        self.currentSort(sortName);
        self.navigateTo(self.currentPage(), self.currentFilter(), self.currentSort(), self.currentSortType());
    };
    self.load = function (page, filter, sort, sortType) {
        //if (page > 0 && page <= self.pageCount()) {
        console.log("page: " + page + " / " + self.pageCount());
        self.currentPage(page);
        self.currentFilter(filter);
        self.currentSort(sort);         // Member to sort by
        self.currentSortType(sortType); // ASC or DESC

        //self.currentFilter()
        //(_url, _sortName, _sortType, _filterName, _page, successCallback, errorCallback)
        $.loadData(url, self.currentSort(), self.currentSortType(), self.currentFilter(), self.currentPage(), self.pageSize(), function (data) {
            //console.log(self.currentSort() + " - " + self.currentSortType());
            //console.log(data);
            //console.log("loaded page # " + self.currentPage());
            //console.log(data);
            self.List(data); //Put the response in ObservableArray
            //self._itemCount(self.List().length);
            //console.log(self._itemCount());


            //console.log(data.length);
            //$('.editable').editable({
            //    ajaxOptions: {
            //        type: 'put',
            //        dataType: 'json'
            //    },
            //    send: 'always',
            //    type: 'text',
            //    url: url+'/Put',
            //    params: function (params) {  //params already contain `name`, `value` and `pk`
            //        var data = {
            //            'id': params.pk,
            //            'Name': params.value,
            //            'Description': ''
            //        };
            //        //console.log(JSON.stringify(data));
            //        //alert(JSON.stringify(data));
            //        return JSON.stringify(data);
            //    },
            //    success: function (response, newValue) {
            //        console.log(response);
            //        if (response.status == 'error') alert(response.msg); //msg will be shown in editable form
            //        //userModel.set('username', newValue); //update backbone model
            //    }
            //});
        });
        //}
    };
    //self.load(self.currentPage);
    ko.applyBindings(self);
    return self;
};

//function Title(Title) {
//    this.Id = ko.observable(Title.Id);
//    this.Name = ko.observable(Title.Name);
//    this.Description = ko.observable(Title.Description);
//}

$(function () {
    var currCell = $('tr td').first();
    
    $(document)
        .on('mousedown', 'tr td', function (e) {
            currCell = $(this).parent();
            //currCell.css('background-color', '#206CAC');
            //window.location = window.location.pathname + '/Details/' + $(this).parent().attr('id');
        })
        .on('mouseenter', 'tr td', function (e) {
            $(this).parent()
                .css('background-color', '#206CAC');
            //.find(".edit-details-delete div").show();
        })
        .on('mouseleave', 'tr td', function (e) {
            $(this).parent()
                .css('background-color', 'transparent');
            //.find(".edit-details-delete div").hide();
        })
        .keydown(function (e) {
            var c = undefined;
            switch (e.which) {
                case 37: // left
                    break;
                case 38: // up
                    c = currCell.closest('tr').parent().prev();//.find('td:eq(' + currCell.index() + ')');
                    //todo: select item in table / list
                    break;
                case 39: // right
                    break;
                case 40: // down
                    //todo: select item in table / list
                    c = currCell.closest('tr').parent().next();//.find('td:eq(' + currCell.index() + ')');
                    break;
                default: return; // exit this handler for other keys
            }
            console.log(c);
            //console.log(currCell.index());
            //e.preventDefault(); // prevent the default action (scroll / move caret)
        });

    $("#pagination-controller").submit(function (e) {
        e.preventDefault();
    });

    $(".go-to").keypress(function (e) {
        if (e.keyCode === 13) {
            var page = indexVM.currentPage();
            page = parseInt($(this).val());
            if (isNaN(page)) {
                //alert(indexVM.currentPage());
                page = indexVM.currentPage();//indexVM.currentPage(2);
                //alert(indexVM.currentPage());
            }
            page = parseInt(page + 1);
            //indexVM.load(page);
            indexVM.navigateTo(page);
        }
    });

    $("#page-size").keyup(function (e) {
        var pageSize = parseInt($(this).val());
        if (!isNaN(pageSize) && pageSize >= 1 && pageSize <= 50) {
            indexVM.pageSize(pageSize);
            console.log(pageSize);
        }
        else {
            indexVM.pageSize(10);
        }
    });

    $(document).on("modalClosed", function (e, d) {
        //console.log(e);
        //console.log(d);
        //indexVM.load();
        //indexVM.load();
        indexVM.loadNextPage();
    });
});


