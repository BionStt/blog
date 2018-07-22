var blogStoryManager = {
    tags: [],
    publishText: 'Published',
    initialize: function (tags) {
        this.tags = tags;
        this.initializeShortCuts();

        tinyMCE.baseURL = 'https://cdn.jsdelivr.net/npm/tinymce@4.7.13/';

        tinymce.init({
            selector: '.editor',
            height: 500,
            menubar: true,
            branding: false,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table contextmenu paste codesample'
            ],
            toolbar:
                'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | codesample',
            codesample_languages: [
                {text: 'HTML/XML', value: 'markup'},
                {text: 'JavaScript', value: 'javascript'},
                {text: 'CSS', value: 'css'},
                {text: 'C', value: 'c'},
                {text: 'C#', value: 'csharp'},
                {text: 'ASP.NET MVC', value: 'aspnet'},
                {text: 'Bash', value: 'bash'},
                {text: 'Powershell', value: 'powershell'},
                {text: 'DockerFile', value: 'docker'},
                {text: 'Yaml', value: 'yaml'}

            ],
            setup: function (editor) {
                editor.shortcuts.add("ctrl+s", "save story", function () {
                    $("#edit-story").submit();
                });
                editor.on('FullscreenStateChanged', function (editor) {
                    var form = $("#edit-story");
                    var fullscreenValue = "#fullscreen";

                    var formCurrentAction = form.attr("action");

                    if (editor.state) {
                        window.location.hash = fullscreenValue;
                        form.attr("action", formCurrentAction + window.location.hash);
                    } else {
                        window.location.hash = "";
                        form.attr("action", formCurrentAction.replace(fullscreenValue, ""));
                    }
                });

                editor.on('init', function () {
                    if (window.location.hash === '#fullscreen') {
                        tinyMCE.execCommand('mceFullScreen');
                    }
                });
            }
        });

        this.initializeTagsSelector();
        this.initializeActions();
    },
    initializeTagsSelector: function () {
        if (!$('.tag-editor').length)
            return;

        this.tagsSelectize = $('.tag-editor').selectize({
            plugins: ['remove_button'],
            delimiter: ',',
            maxItems: 3,
            valueField: 'id',
            labelField: 'name',
            searchField: 'name',
            options: this.tags,
            create: function (input, callback) {
                var request = JSON.stringify({name: input});
                $.ajax({
                    type: "POST",
                    url: "/author/tags",
                    data: request,
                    contentType: "application/json",
                    success: function (data) {
                        var newItem = {id: data.id, name: data.name};
                        callback(newItem);
                        blogStoryManager.tags.push(newItem);
                    },
                    error: function (data) {
                        alert(data);
                    }
                });
            }
        });
    },
    initializeActions: function () {
        $('#publish-btn').click(function () {
            var $button = $(this);
            var storyId = $("#Id").val();
            var value = $button.attr('set');
            $.ajax({
                type: "PATCH",
                url: "/author/stories/" + storyId + "?isPublished=" + value,
                contentType: "application/json",
                success: function (data) {
                    if (data) {
                        if (data.redirect) {
                            location.replace(data.redirect);
                        } else {
                            location.reload();
                        }
                    }
                },
                error: function (data) {
                    alert(data);
                }
            });
        });

        $("#update-access-token-btn").click(this.updateAccessToken);
        $("#remove-access-token-btn").click(this.removeAccessToken);
    },
    initializeShortCuts: function () {
        $(window).bind('keydown',
            function (event) {
                if (event.ctrlKey || event.metaKey) {
                    switch (String.fromCharCode(event.which).toLowerCase()) {
                        case 's':
                            event.preventDefault();
                            $("#edit-story").submit();
                            break;
                    }
                }
            });
    },
    updateAccessToken: function () {
        var id = $("#Id").val();

        $.ajax({
            type: "POST",
            url: "/author/stories/" + id + "/accesstoken",
            success: function () {
                document.location.reload();
            }
        });
    },
    removeAccessToken: function () {
        var id = $("#Id").val();

        $.ajax({
            type: "DELETE",
            url: "/author/stories/" + id + "/accesstoken",
            success: function () {
                document.location.reload();
            }
        });
    }
};