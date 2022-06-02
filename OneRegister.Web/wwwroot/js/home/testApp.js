"use strict"
if (typeof (testApp) === 'undefined') {
    var testApp = {
        method: {
            doBeep: function () {
                testApp.method.appendResult('do you hear any sound?');
                if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.toggleMessageHandler) {
                    window.webkit.messageHandlers.toggleMessageHandler.postMessage({
                        'message': "Hello Apple!"
                    });
                }
            },
            doCamera: function () {
                testApp.method.appendResult('you look nice in camera!');
            },
            doData: function () {
                testApp.method.appendResult('getting data from your phone...');
            },
            appendResult: function (text) {
                $('#resultText').append('\n'+text);
            }
        },
        event: {
            beepClick: function(){
                $('#btnBeep').on('click', function () {
                    testApp.method.doBeep();
                })
            },
            cameraClick: function () {
                $('#btnCamera').on('click', function () {
                    testApp.method.doCamera();
                })
            },
            dataClick: function () {
                $('#btnData').on('click', function () {
                    testApp.method.doData();
                })
            },
            register: function () {
                testApp.event.beepClick();
                testApp.event.cameraClick();
                testApp.event.dataClick();
            }
        }
    }
}

$(function () {
    testApp.event.register();
});