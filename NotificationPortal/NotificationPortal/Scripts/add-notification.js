(function ($) {
    function HomeIndex() {
        var $this = this;

        function initialize() {
            $('#NotificationDescription').summernote({
                focus: false,
                height: 150,
                codemirror: {
                    theme: 'united'
                },
                popover: {},
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['link', ['linkDialogShow', 'unlink']]
                ]
            });
        }

        $this.init = function () {
            initialize();
        };
    }
    $(function () {
        var self = new HomeIndex();
        self.init();
    });
}(jQuery));  