if (typeof (MerchantInbox) === "undefined") {
    var MerchantInbox = {
        config: {
            gridId: 'merchantListGrid'
        },
        dataTable: {
            grid: {},
            init: function () {
                MerchantInbox.dataTable.grid = $('#' + MerchantInbox.config.gridId).DataTable({
                    ajax: {
                        url: 'MerchantInbox',
                        type: 'POST',
                        dataSrc: ""
                    },
                    order:[[1,"asc"]],
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
    MerchantInbox.dataTable.init();
});