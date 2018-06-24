var blogStoryManager = {
    tags: [],
    publishText: 'Published',
    initialize: function (tags) {
        this.tags = tags;
        this.initializeShortCuts();

        tinyMCE.baseURL = '/lib/tinymce';

        tinymce.init({
            selector: '.editor',
            height: 500,
            menubar: true,
            plugins: [
                'advlist autolink lists link image charmap print preview anchor',
                'searchreplace visualblocks code fullscreen',
                'insertdatetime media table contextmenu paste codesample'
            ],
            toolbar:
            'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | codesample',
            codesample_languages: [
                { text: 'HTML/XML', value: 'markup' },
                { text: 'JavaScript', value: 'javascript' },
                { text: 'CSS', value: 'css' },
                { text: 'C', value: 'c' },
                { text: 'C#', value: 'csharp' },
                { text: 'ASP.NET MVC', value: 'aspnet' },
                { text: 'Bash', value: 'bash' },
                { text: 'Powershell', value: 'powershell' },
                { text: 'DockerFile', value: 'docker' },
                { text: 'Yaml', value: 'yaml' }

            ],
            setup: function (editor) {
                editor.shortcuts.add("ctrl+s", "save story", function () { $("#edit-story").submit(); });
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
                var request = JSON.stringify({ name: input });
                $.ajax({
                    type: "POST",
                    url: "/author/tags",
                    data: request,
                    contentType: "application/json",
                    success: function (data) {
                        var newItem = { id: data.id, name: data.name };
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

        $("#share-link-copy").click(blogStoryManager.copyShareLink);
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
    initializeCopyShareLinkEvent: function () {
        var copyData = function (e) {
            var link = $("#share-link-copy").attr('link');
            e.clipboardData.setData('text/plain', link);
            document.removeEventListener('copy', copyData);
            e.preventDefault();
        };


        document.addEventListener('copy', copyData);
    },
    copyShareLink: function (e) {
        var $statusItem = $("#story-status span").text().trim();
        var isPublished = $statusItem === blogStoryManager.publishText;

        blogStoryManager.initializeCopyShareLinkEvent();

        if (isPublished) {
            var link = $("#share-link").attr("href");
            $("#share-link-copy").attr('link', location.origin + link);
            document.execCommand('copy');
        } else {
            if ($("#share-link-copy").attr("link")) {
                document.execCommand('copy');
            }
            else if ($("#AccessToken").val()) {
                var alias = $("#Alias").val();
                var accessToken = $("#AccessToken").val();
                $("#share-link-copy").attr("link", location.origin + '/drafts/' + alias + '?token=' + accessToken);
                document.execCommand('copy');
            }
            else {
                blogStoryManager.createShareLink();
            }
        }
    },
    createShareLink: function () {
        var id = $("#Id").val();

        $.ajax({
            type: "POST",
            url: "/author/stories/" + id + "/sharelinks",
            success: function (data) {
                $("#share-link-copy").attr('link', location.origin + data.link);
                document.execCommand('copy');
            },
            error: function (data) {
                alert(data);
            }
        });

    }
};