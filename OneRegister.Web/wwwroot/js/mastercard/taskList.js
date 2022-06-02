"use strict";
if (typeof (taskList) === 'undefined') {
    var taskList = {
        dataTable: {
            grid: {},
            init: function () {
                var resetButton = function (rowId) {
                    return `<a href="#" onClick="taskList.resetTask('${rowId}')" class="btn btn-link mx-1"><i class="fas fa-redo-alt text-info"></i></a>`;
                };
                taskList.dataTable.grid = $('#MCTaskList').DataTable({
                    serverSide: true,
                    processing: true,
                    searchDelay: 700,
                    ordering: false,
                    ajax: {
                        url: 'TaskList',
                        type: 'POST'
                    },
                    language: {
                        zeroRecords: 'No matching records found'
                    },
                    columns: [
                        { data: "id", visible: false },
                        {
                            data: "state",
                            render: function (data, type, row, meta) {
                                switch (data) {
                                    case 'Complete' :
                                        return '<i class="fas fa-thumbs-up text-success mx-1"></i>';
                                    case 'Fetched':
                                        return '<i class="fas fa-compress-alt text-warning mx-1"></i>';
                                    case 'InProgress':
                                        return '<i class="fas fa-hourglass-half text-primary mx-1"></i>';
                                    default:
                                        return '<i class="fas fa-times text-danger mx-1"></i>';
                                }
                            }
                        },
                        {
                            data: "refId",
                            render: function (data, type, row, meta) {
                                if (row.state === 'Complete' || row.state === 'InProgress') {
                                    return data;
                                } else {
                                    return data + resetButton(row.id);
                                }
                            }
                        },
                        { data: "name" },
                        { data: "inquiryName" },
                        { data: "modifiedAt" },
                        { data: "source" },
                        { data: "createdAt" },
                        { data: "result" },
                        { data: "errorSource" },
                        { data: "errorCode" },
                        { data: "state" },
                        { data: "refId2" }
                    ]
                });
            }
        },
        resetTask: function (id) {
            app.confirmModal.setTitle("InquiryTask - Reset");
            app.confirmModal.setBody("are you sure to reset this Inquiry task?");
            app.confirmModal.paramId = id;
            app.confirmModal.confirmAction = taskList.doResetTask;
            app.confirmModal.popUp();
        },
        doResetTask: function () {
            $.ajax({
                url: 'ResetTask',
                type: 'POST',
                data: { "taskId": app.confirmModal.paramId },
                dataType: 'JSON',
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("task recycled successfully");
                        taskList.dataTable.grid.ajax.reload();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function () {
                    app.confirmModal.dismiss();
                }
            });
        }
    }
}

$(function () {
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
        app.alert.ajax.show(message);
    };
    taskList.dataTable.init();
});