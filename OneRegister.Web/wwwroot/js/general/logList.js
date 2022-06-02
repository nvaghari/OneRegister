var logListObj = {
    config: {
        gridId: 'logTable'
    },
    dataTable: {
        grid: {},
        data: {},
        init: function () {
            logListObj.dataTable.grid = $('#' + logListObj.config.gridId).DataTable({
                ordering: false,
                data: logListObj.dataTable.data,
                columns: [
                    { data: "time" },
                    {
                        data: "level",
                        render: function (data, type, row, meta) {
                            if (data == 'DBG') return '<i class="fas fa-search text-primary pr-2"></i>'+data;
                            if (data == 'INF') return '<i class="fas fa-file-alt text-info pr-2"></i>'+data;
                            if (data == 'WRN') return '<i class="fas fa-exclamation-triangle text-warning pr-2"></i>'+data;
                            if (data == 'ERR') return '<i class="fas fa-bug text-danger pr-2"></i>'+data;
                            if (data == 'FTL') return '<i class="fas fa-skull text-dark pr-2"></i>'+data;

                            return data;
                        }

                    },
                    { data: "message" },
                    { data: "session" },
                    { data: "remoteAddress" },
                    { data: "user" }
                ]
            });
        }
    },
    event: {
        registerShowButton: function () {
            $('#showLogBtn').on('click', function () {
                logListObj.method.getLogs($('#FileListId').val());
            });
        }
    },
    method: {
        getLogs: function (logName) {
            $.post(app.properties.BaseURL + "Audit/GetLog", { 'logName': logName }, function (data) {
                logListObj.dataTable.data = data;
                logListObj.dataTable.grid.destroy();
                logListObj.dataTable.init();
            }, 'Json');
        }
    }
};

$(function () {
    logListObj.dataTable.init();
    logListObj.event.registerShowButton();
});