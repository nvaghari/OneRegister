"use strict"
if (typeof (list) === 'undefined') {
    var list = {
        initTree: function () {
            var tree = $("#orgTree").tree({
                primaryKey: 'id',
                dataSource: {
                    url: 'Organization/GetTree',
                    method: 'POST',
                    dataType: 'JSON'
                },
                uiLibrary: 'bootstrap4',
                checkboxes: true
            });
        }
    }
}

$(function () {
    list.initTree();
});