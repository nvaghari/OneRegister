"use strict"
if (typeof (Permission) === 'undefined') {
    var Permission = {
        config: {
            gridId: 'permissionListGrid'
        },
        dataTable: {
            grid: {},
            init: function () {
                Permission.dataTable.grid = $('#' + Permission.config.gridId).DataTable({
                    ajax: {
                        url: "GetPermissions",
                        type: "POST",
                        dataSrc: ""
                    },
                    order: [[2, "asc"]],
                    columns: [
                        { data: "id", visible: false },
                        { data: "name" },
                        { data: "className" },
                        { data: "methodName" },
                        { data: "attributeType" },
                        { data: "organizationName" }
                    ]
                });
            }
            
        },
        method: {
            addRole: function () {
                var isValid = app.validation.validateForm('roleForm');
                if (isValid) {
                    var formData = new FormData(document.getElementById('roleForm'));
                    app.ajax.formData(formData, 'AddRole', Permission.method.addRoleAjaxSuccess, true);
                }
            },
            addRoleAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Role is added successfully');
                    Permission.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            },
            removeRole: function (id) {
                var formData = new FormData();
                formData.append('roleId', id);
                app.ajax.formData(formData, "RemoveRole", Permission.method.removeRoleAjaxSuccess, true);
            },
            removeRoleAjaxSuccess: function (data) {
                if (data.isSuccessful) {
                    app.alert.showSuccessAutoHide('Role is removed successfully');
                    Permission.dataTable.grid.ajax.reload();
                } else {
                    app.validation.checkUnsuccessfulResponse(data);
                }
            }
        },
        events: {
            addBtnClick: function () {
                $('#roleAddBtn').on('click', function () {
                    Permission.method.addRole();
                });
            },
            registerAll: function () {
                //app.event.tooltip();
                Permission.dataTable.init();
                //Permission.events.addBtnClick();
            }
        }
    };
}

$(function () {
    Permission.events.registerAll();
});