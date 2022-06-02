"use strict"
var UserList = {
    config: {
        gridId: 'userListGrid'
    },
    tree: {},
    dataTable: {
        grid: {},
        init: function () {
            UserList.dataTable.grid = $('#' + UserList.config.gridId).DataTable({
                ajax: {
                    url: "GetUsers",
                    type: "POST",
                    dataSrc: ""
                },
                order: [[2, "asc"]],
                columns: [
                    {
                        data: null,
                        defaultContent: "",
                        orderable: false,
                        searchable: false,
                        render: UserList.dataTable.actionRender
                    },
                    { data: "id", visible: false },
                    { data: "userName" },
                    { data: "name" },
                    { data: "email" },
                    { data: "phone" },
                    { data: "organization" },
                    {
                        data: "isEnable",
                        render: function (data, type, row, meta) {
                            if (data == true) {
                                return "Active";
                            } else {
                                return "Inactive";
                            }
                        }
                    }
                ]
            });
        },
        actionRender: function (data, type, row, meta) {
            var actions = "";
            if (row.isEnable) {
                actions += `<a class="btn" title="Deactivate User" onclick="UserList.method.deleteUser('${row.id}','${row.userName}')"><i class="fas fa-user text-primary"></i></a>`;
                actions += `<a class="btn" title="Reset Password" onclick="UserList.method.resetPassword('${row.id}','${row.userName}')"><i class="fa fa-sync text-primary"></i></a>`;
                actions += `<a class="btn" title="Role Management" onclick="UserList.method.addRole('${row.id}','${row.userName}')"><i class="fas fa-users text-primary"></i></a>`;
                actions += `<a class="btn" title="Organization Management" onclick="UserList.method.changeOrganization('${row.id}','${row.userName}')"><i class="fas fa-sitemap text-primary"></i></a>`;
            } else {
                actions += `<a class="btn" title="Activate User" onclick="UserList.method.restoreUser('${row.id}','${row.userName}')"><i class="fas fa-user-slash text-warning"></i></a>`;
                actions += `<a class="btn" title="Role Management" onclick="UserList.method.addRole('${row.id}','${row.userName}')"><i class="fas fa-users text-warning"></i></a>`;
            }
            return actions;
        }
    },
    method: {
        resetPassword: function (userId, userName) {
            app.confirmModal.setParam(userId);
            app.confirmModal.setTitle("Reset User Password");
            app.confirmModal.setBody('Do you want to reset <span class="font-weight-bold">' + userName + '</span> password? The default password is: 123@Abc');
            app.confirmModal.confirmAction = UserList.method.doResetPassword;
            app.confirmModal.popUp();
        },
        doResetPassword: function () {

            $.ajax({
                url: "ResetUser",
                type: "POST",
                data: { "userId": app.confirmModal.paramId },
                dataType: "Json",
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("The password was reset successfully");
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function (xhr, status) {
                    app.confirmModal.dismiss();
                }
            });
        },
        deleteUser: function (userId, userName) {
            app.confirmModal.setParam(userId);
            app.confirmModal.setTitle("Deactivate User");
            app.confirmModal.setBody('Do you want to deactivate <span class="font-weight-bold">' + userName + '</span>?');
            app.confirmModal.popUp();
            app.confirmModal.confirmAction = UserList.method.doDeleteUser;
        },
        doDeleteUser: function () {

            $.ajax({
                url: "DeleteUser",
                type: "POST",
                data: { "userId": app.confirmModal.paramId },
                dataType: "Json",
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("The user was deactivated successfully");
                        UserList.dataTable.grid.ajax.reload();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function (xhr, status) {
                    app.confirmModal.dismiss();
                }
            });
        },
        restoreUser: function (userId, userName) {
            app.confirmModal.setParam(userId);
            app.confirmModal.setTitle("Activate User");
            app.confirmModal.setBody('Do you want to activate <span class="font-weight-bold">' + userName + '</span>?');
            app.confirmModal.popUp();
            app.confirmModal.confirmAction = UserList.method.doRestoreUser;
        },
        doRestoreUser: function () {
            $.ajax({
                url: "RestoreUser",
                type: "POST",
                data: { "userId": app.confirmModal.paramId },
                dataType: "Json",
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("The user was activated successfully");
                        UserList.dataTable.grid.ajax.reload();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function (xhr, status) {
                    app.confirmModal.dismiss();
                }
            });
        },
        addRole: function (userId, userName) {
            var modal = app.confirmModal;
            modal.setParam(userId);
            modal.setTitle(`Role Management (${userName})`);
            modal.confirmAction = UserList.method.doAddRole;
            modal.setBody('<div id="roleTree"></div>');
            UserList.method.initRolesTree(userId);
            modal.popUp();
        },
        initRolesTree: function (userId) {
            UserList.tree = $("#roleTree").tree({
                primaryKey: 'id',
                dataSource: {
                    url: 'GetUserRoleTree',
                    data: { "userId": userId },
                    method: 'POST',
                    dataType: 'JSON'
                },
                uiLibrary: 'bootstrap4',
                checkboxes: true
            });
        },
        doAddRole: function () {
            $.ajax({
                url: 'UpdateUserRoles',
                type: 'POST',
                dataType: 'JSON',
                data: { 'userId': app.confirmModal.paramId, 'roles': UserList.tree.getCheckedNodes() },
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("Role(s) Added Successfully");
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                err: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function (xhr, status) {
                    app.confirmModal.dismiss();
                }
            });
        },
        makeUserRolesForm: function (roles) {
            var body = '<div id="roleList" class="px-3">';
            for (var role of roles) {
                body += '<div class="form-check">';
                if (role.isAssigned) {
                    body += '<input checked class="form-check-input" type="checkbox" value="' + role.id + '" />';
                } else {
                    body += '<input class="form-check-input" type="checkbox" value="' + role.id + '" />';
                }
                body += '<label>' + role.name + ' (' + role.organization + ')' + '</label>';
                body += '</div>';
            }
            body += '</div>';
            return body;
        },
        changeOrganization: function (userId, userName) {
            var modal = app.confirmModal;
            modal.setParam(userId);
            modal.setTitle(`Change Organization (${userName})`);
            modal.confirmAction = UserList.method.doChangeOrganization;
            $.ajax({
                url: 'GetOrganizations',
                type: 'POST',
                data: { 'userId': userId },
                dataType: 'Json',
                success: function (data) {
                    var body = '<div id="orgtree"></div>';
                    modal.setBody(body);
                    ninotree.single.init('orgtree', data);
                },
                error: function (err) {
                    modal.setBody("Error: " + err.statusText);
                },
                complete: function (xhr, status) {
                    modal.popUp();
                }
            });
        },
        doChangeOrganization: function () {
            $.ajax({
                url: "ChangeUserOrganization",
                type: "POST",
                data: { "userId": app.confirmModal.paramId, "orgId": ninotree.single.getSelected() },
                dataType: "Json",
                success: function (data) {
                    if (data.isSuccessful) {
                        app.alert.showSuccessAutoHide("The Organization was changed successfully");
                        UserList.dataTable.grid.ajax.reload();
                    } else {
                        app.alert.showErrorAutoHide(data.message);
                    }
                },
                error: function (err) {
                    app.alert.showAjaxErrorAutoHide(err);
                },
                complete: function (xhr, status) {
                    app.confirmModal.dismiss();
                }
            });
        }
    }
};

$(function () {
    UserList.dataTable.init();
});