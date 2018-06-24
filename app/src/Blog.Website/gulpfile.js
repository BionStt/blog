/// <binding BeforeBuild='min' Clean='clean' ProjectOpened='min' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require("gulp-rename"),
    strip = require('gulp-strip-comments');

var paths = {
    webroot: "./wwwroot/",
    uiRoot: "./UI/",
    uiPlugins: "./UI/plugins/",
    uiCustomPlugins: "./UI/custom-plugins/",
    uiCss: "./UI/css/",
    uiJs: "./UI/js/"
};

paths.adminJs = paths.webroot + "admin.min.js";
paths.adminCss = paths.webroot + "admin.min.css";

paths.frontJs = paths.webroot + "front.min.js";
paths.frontCss = paths.webroot + "front.min.css";

paths.deploy = "";


var adminJsFiles = [
    paths.uiPlugins + 'jquery/dist/jquery.min.js',
    paths.uiPlugins + 'tether/dist/js/tether.min.js',
    paths.uiPlugins + 'bootstrap/dist/js/bootstrap.min.js',
    paths.uiPlugins + 'responsive-bootstrap-toolkit/dist/bootstrap-toolkit.min.js',
    paths.uiPlugins + 'jquery-validation/dist/jquery.validate.min.js',
    paths.uiPlugins + 'jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js',
    paths.uiPlugins + 'metisMenu/dist/metisMenu.min.js',
    paths.uiPlugins + 'nprogress/nprogress.js',
    paths.uiPlugins + 'Sortable/Sortable.min.js',
    paths.uiPlugins + 'tinymce/tinymce.min.js',
    paths.uiPlugins + 'microplugin/src/microplugin.js',
    paths.uiPlugins + 'sifter/sifter.min.js',
    paths.uiPlugins + 'selectize/dist/js/selectize.min.js',
    paths.uiCustomPlugins + 'prism.min.js',
    paths.uiJs + 'app.js'
];

var adminCssFiles = [
    paths.uiPlugins + 'bootstrap/dist/css/bootstrap-reboot.min.css',
    paths.uiPlugins + 'bootstrap/dist/css/bootstrap-grid.min.css',
    paths.uiPlugins + 'bootstrap/dist/css/bootstrap-flex.min.css',
    paths.uiPlugins + 'bootstrap/dist/css/bootstrap.min.css',
    paths.uiPlugins + 'animate.css/animate.min.css',
    paths.uiPlugins + 'font-awesome/css/font-awesome.min.css',
    paths.uiPlugins + 'metisMenu/dist/metisMenu.min.css',
    paths.uiPlugins + 'selectize/dist/css/selectize.css',
    paths.uiCss + 'prism.vs.css',
    paths.uiCss + 'app.css'
];


gulp.task("min:admin-managers", function () {
    var files = [
        'blog-story-index',
        'blog-story-edit',
        'category-index',
        'category-edit',
        'tag-index'
    ];

    files.forEach(function (file) {
        gulp.src(paths.uiJs + file + '.js')
            .pipe(uglify())
            .pipe(concat(file + '.min.js'))
            .pipe(gulp.dest(paths.webroot));
    });

    return 0;
});

gulp.task("min:admin-base", function () {
    return gulp.src(adminJsFiles)
        .pipe(concat(paths.adminJs))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:admin-js", ["min:admin-base", "min:admin-managers"]);

gulp.task("clean-admin-js", function (cb) {
    rimraf(paths.adminJs, cb);
});

gulp.task("min:admin-css", function () {
    return gulp.src(adminCssFiles)
        .pipe(concat(paths.adminCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("clean-admin-css", function (cb) {
    rimraf(paths.adminCss, cb);
});

gulp.task("min-admin", ["min:admin-js", "min:admin-css"]);
gulp.task("clean-admin", ["clean-admin-js", "clean-admin-css"]);


var frontJsFiles = [
    paths.uiJs + 'jquery-3.2.1.min.js',
    paths.uiCustomPlugins + 'prism.min.js',
    paths.uiPlugins + 'masonry/dist/masonry.pkgd.min.js',
    paths.uiPlugins + 'imagesloaded/imagesloaded.pkgd.min.js',
    paths.uiPlugins + 'magnific-popup/dist/jquery.magnific-popup.min.js',
    paths.uiJs + 'jquery.disqusloader.js',
    paths.uiJs + 'frontend.js',
    paths.uiJs + 'google-analytic.js'
];

var frontJsFilesCopy = [
    paths.uiPlugins + 'lozad/dist/lozad.min.js'
];

var frontCssFiles = [
    paths.uiCss + 'frontend.css',
    paths.uiCss + 'prism.vs.css',
    paths.uiPlugins + 'magnific-popup/dist/magnific-popup.css',
    paths.uiCss + 'custom-frontend.css'
];

gulp.task("min-front-css", function () {
    return gulp.src(frontCssFiles)
        .pipe(concat(paths.frontCss))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("clean-front-css", function (cb) {
    rimraf(paths.frontCss, cb);
});

gulp.task("min-front-js", function () {
    return gulp.src(frontJsFiles)
               .pipe(concat(paths.frontJs))
               .pipe(strip())
               .pipe(uglify())
               .pipe(gulp.dest("."));
});

gulp.task("clean-front-js", function (cb) {
    rimraf(paths.frontJs, cb);
});

gulp.task("copy-min", function () {
    return gulp.src(frontJsFilesCopy)
               .pipe(gulp.dest(paths.webroot));
});

gulp.task("min-front", ["min-front-js", "min-front-css"]);
gulp.task("clean-front", ["clean-front-js", "clean-front-css"]);

gulp.task("min", ["min-admin", "min-front"]);
gulp.task("clean", ["clean-admin", "clean-front"]);