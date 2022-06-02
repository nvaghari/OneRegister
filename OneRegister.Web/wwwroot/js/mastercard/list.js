"use strict";
if (typeof (list) === 'undefined') {
    var list = {
        dataTable: {
            grid: {},
            init: function () {
                list.dataTable.grid = $('#customerList').DataTable({
                    serverSide: true,
                    processing: true,
                    searchDelay: 700,
                    ordering: false,
                    ajax: {
                        url: 'List',
                        type: 'POST'
                    },
                    language: {
                        zeroRecords: 'No matching records found'
                    },
                    columns: [
                        {
                            data: "formNo",
                            render: function (data, type, row, meta) {
                                return '<a href="Edit/' + row.formNo + '">' + data + '</a>';
                            }
                        },
                        { data: "formType" },
                        { data: "formStatus" },
                        { data: "channel" },
                        { data: "fullName" },
                        { data: "mobileNo" },
                        { data: "idNo" },
                        { data: "entityCIF" },
                        { data: "listServicePackages" },
                    ]
                });
            }
        }
    }
}

$(function () {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        app.alert.ajax.show(message);
    };
    list.dataTable.init();
});