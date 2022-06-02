if (typeof(agroListObj) === 'undefined') {
    var agroListObj = {
        config: {
            gridId: 'agroListGrid'
        },
        dataTable: {
            grid: {},
            init: function () {
                agroListObj.dataTable.grid = $('#' + agroListObj.config.gridId).DataTable({
                    serverSide: true,
                    processing: true,
                    searchDelay: 700,
                    ordering: false,
                    ajax: {
                        url: 'AgropreneurList',
                        type: 'POST'
                    },
                    language: {
                        zeroRecords: 'No matching records found <a href="Register">Click here to register Agropreneur<a>'
                    },
                    columns: [
                        { data: "id", visible: false },
                        {
                            data: "hasPicture",
                            render: function (data, type, row, meta) {
                                if (data) {
                                    return '<img src="GetThumbnail?agroId=' + row.id + '" width=50 height=70></img>'
                                } else {
                                    return '<img src="../pic/thumbnail.jpg" width=50 height=70></img>'
                                }
                            }
                        },
                        {
                            data: "plotNo",
                            searchable: true
                        },
                        {
                            data: "name",
                            render: function (data, type, row, meta) {
                                return '<a href="Edit/' + row.id + '">' + data + '</a>';
                            }
                        },
                        { data: "company" },
                        { data: "gender" },
                        { data: "nationality" },
                        {
                            data: "hasPicture",
                            render: function (data, type, row, meta) {
                                if (data) {
                                    return '<i class="fa fa-check text-success"></i>';
                                } else {
                                    return '<i class="fa fa-times text-danger"></i>';
                                }

                            }
                        },
                        { data: "identityType" },
                        { data: "identityNumber" },
                        { data: "status" }
                    ]
                });
            }
        }
    };
}

$(function () {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        app.alert.showError(message);
    };
    agroListObj.dataTable.init();
});