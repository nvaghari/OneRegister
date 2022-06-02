if (typeof (merchantListObj) === "undefined") {
    var merchantListObj = {
        config: {
            gridId: 'merchantListGrid'
        },
        dataTable: {
            grid: {},
            init: function () {
                merchantListObj.dataTable.grid = $('#' + merchantListObj.config.gridId).DataTable({
                    serverSide: true,
                    processing: true,
                    searchDelay: 700,
                    ordering: false,
                    ajax: {
                        url: 'MerchantList',
                        type: 'POST'
                    },
                    language: {
                        zeroRecords: 'No matching records found <a href="Register">Click here to register Merchant<a>'
                    },
                    columns: [
                        { data: "id", visible: false },
                        { data: "formNo" },
                        { data: "status" },
                        {
                            data: "registeredBusiness",
                            render: function (data, type, row, meta) {
                                return '<a href="Register/' + row.formNo + '">' + data + '</a>';
                            }
                        },
                        { data: "businessNo" },
                        { data: "businessType" },
                        { data: "salesperson" },
                        { data: "contactName" },
                        { data: "designation" },
                        { data: "mobileNo" }
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
    merchantListObj.dataTable.init();
});