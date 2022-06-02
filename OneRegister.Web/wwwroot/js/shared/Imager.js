// v2.0 by Nader Vaghari
var Imager = {
    config: {
        compress: {
            quality: 0.8,
            maxWidth: 500,
            maxHeight: 700,
            convertSize: 100,
            success: function (result) {
                Imager.properties.photoC = result;
                var srcUrl = URL.createObjectURL(result);
                $('#photo').attr('src', srcUrl);
            },
            error: function (err) {
                app.alert.showErrorAutoHide('Error on compressing the image: ' + err.message);
            }
        },
        thumbnail: {
            quality: 0.8,
            width: 50,
            heigh: 70,
            convertSize: 100,
            success: function (result) {
                Imager.properties.photoT = result;
            },
            error: function (err) {
                app.alert.showErrorAutoHide('Error on compressing the image for thumbnail: ' + err.message);
            }
        }
    },
    properties: {
        photo: undefined,
        photoC: null,
        photoT: null,
        photoName: 'error.jpg',
        crp: {},
        width: 300,
        height: 400,
        margin: 50
    },
    methods: {
        isValid: function (photo) {
            if (photo.type != 'image/jpeg') return false;
            Imager.properties.photo = undefined;
            return true;
        },
        toString: function (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onloadend = function () {
                var base64result = reader.result.split(',')[1];
                $('#PhotoStr').val(base64result);
                $('#PhotoName').val(Imager.properties.photoName);
            }
        },
        thumbnailToString: function (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onloadend = function () {
                var base64result = reader.result.split(',')[1];
                $('#ThumbnailStr').val(base64result);
            }
        },
        initCropper: function (file) {
            if (typeof Imager.crp != 'undefined') {
                Imager.crp.croppie('destroy');
            }
            Imager.crp = $('#cropper').croppie({
                enableExif: true,
                viewport: {
                    width: Imager.properties.width,
                    height: Imager.properties.height,
                    type: 'square'
                },
                boundary: {
                    width: Imager.properties.width + Imager.properties.margin,
                    height: Imager.properties.height + Imager.properties.margin
                }
            });
            Imager.crp.croppie('bind', {
                url: file
            })
        },
        compress: function (file) {
            new Compressor(file, Imager.config.compress);
            new Compressor(file, Imager.config.thumbnail);
        },
        crop: function () {
            Imager.crp.croppie('result', {
                type: 'blob',
                format: 'jpeg',
                size: 'viewport'
            }).then(function (resp) {
                Imager.methods.compress(resp);
            });

        }
    }
}