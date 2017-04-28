(function ($) {  
    function HomeIndex() {  
        var $this = this;  
  
        function initialize() {  
            $('#NotificationDescription').summernote({  
                focus: true,  
                height: 150,    
                codemirror: {   
                    theme: 'united'  
                },
                toolbar: [
                    ['style', ['bold', 'italic', 'underline']], 
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['link', ['linkDialogShow', 'unlink']]
                ]
            });  
        }  
  
        $this.init = function () {  
            initialize();  
        }  
    }  
    $(function () {  
        var self = new HomeIndex();  
        self.init();  
    })  
}(jQuery))  