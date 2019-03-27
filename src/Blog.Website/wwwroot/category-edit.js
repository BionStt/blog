var categoryManager = {
    categories: [],
    categorySelectize: {},
    initialize: function (categories) {
        this.categories = categories;
        this.initializeSelect();
    },
    initializeSelect: function () {
        if (!$('.category-editor').length)
            return;

        this.categorySelectize = $('.category-editor').selectize({
            plugins: ['remove_button'],
            valueField: 'id',
            labelField: 'name',
            searchField: 'name',
            options: this.categories
        });
        if ($("#ParentId").attr("parent-id"))
            this.categorySelectize.setValue($("#ParentId").attr("parent-id"));
    }
};