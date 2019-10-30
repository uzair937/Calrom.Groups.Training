const gulp = require('gulp');

gulp.task('lint-less', function lintCssTask() {
  const gulpStylelint = require('gulp-stylelint');

  return gulp
    .src('**/*.less')
    .pipe(gulpStylelint({
      reporters: [
        {formatter: 'string', console: true}
      ]
    }));
});

gulp.task('lint-watch', function() {
    gulp.watch('**/*.less', gulp.series('lint-less')); 
});