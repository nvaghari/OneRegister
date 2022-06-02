if (typeof (settingApp) === 'undefined') {
    var settingApp = {
        method: {
            syncCodeList: function () {
                app.ajax.postSimple('SyncCodeList', {}, true);
            },
            syncPermissions : function() {
                app.ajax.postSimple('SyncPermissions', {}, true);
            },
            updateOrganizationPaths: function () {
                app.ajax.postSimple('/Organization/UpdatePaths', {}, true);
            }

        },
        event: {
            syncCodeListBtn: function () {
                $('#syncCodeListBtn').on('click', function () {
                    settingApp.method.syncCodeList();
                });
            },
            syncPermissionsBtn: function () {
                $('#syncPermissionsBtn').on('click', function () {
                    settingApp.method.syncPermissions();
                });
            },
            updateOrganizationPaths: function () {
                $('#updateOrgPathsBtn').on('click', function () {
                    settingApp.method.updateOrganizationPaths();
                });
            },
            registerAll: function () {
                var e = settingApp.event;

                e.syncCodeListBtn();
                e.syncPermissionsBtn();
                e.updateOrganizationPaths();
            }
        }
    }
}

$(function () {
    settingApp.event.registerAll();
});