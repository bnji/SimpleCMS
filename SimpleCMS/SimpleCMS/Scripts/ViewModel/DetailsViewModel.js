function DetailsViewModel() {
    var self = this;
    self.isBound = false;
    self.data = [];
    self.clear = function (key) {
        //ko.utils.arrayForEach(this.data, function (item) {
        //    item([]);
        //});
        self.isBound = false;
        var temp = self.data[key];
        if (temp) {
            temp.removeAll();
        }
        return self;
    };
    self.add = function (key, json) {
        var mapping = ko.mapping.fromJS([]);
        ko.mapping.fromJS(json, mapping);
        //console.log(mapping());
        self.data[key] = mapping;
        return self;
    };
    self.load = function (key, json) {
        //http://knockoutjs.com/documentation/plugins-mapping.html
        ko.utils.arrayFirst(self.data[key](), function (item) {
            //console.log(item);
            //console.log(json);
            //try {
            //console.log(item);
            if (item.Id() === json.Id) {
                console.log("OK");
                // Update the item with the new json data
                ko.mapping.fromJS(json, {}, item);
            }
            //} catch (ex) {
            //    $.showError("Model passed into DetailsViewModel is missing an Id property", "");
            //}
        });
        return self;
    };
    self.apply = function () {
        ko.applyBindings(self);
        self.isBound = true;
        return self;
    };
    self.reload = function (key, json) {
        var node = $("#" + key)[0];
        if (self.data[key]) {
            self.data[key].removeAll();
            ko.cleanNode(node);
            console.log("cleared node " + node);
        }
        //else {
            var mapping = ko.mapping.fromJS([]);
            ko.mapping.fromJS(json, mapping);
            self.data[key] = mapping;
        //}
        console.log(self.data[key]());
        //self.load(key, json);
        ko.utils.arrayFirst(self.data[key](), function (item) {
            //console.log(item);
            //console.log(json);
            if (item.Id() === json.Id) {
                console.log("OK");
                // Update the item with the new json data
                ko.mapping.fromJS(json, {}, item);
            }
        });

        ko.applyBindings(self, node);
        return self;
    };
    self.applyOnNode = function (nodeId) {
        var node = $(nodeId)[0];
        if (self.isBound === true) {
            ko.cleanNode(node);
        }

        ko.applyBindings(self, node);
        self.isBound = true;
        return self;
    };
    return self;
};