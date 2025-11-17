import type { CourseDto } from '../types/gameplay';

interface LessonListProps {
  courses: CourseDto[];
  onCompleteLesson: (lessonId: number) => void;
  isProcessing: boolean;
  onSelectLesson?: (lessonId: number) => void;
}

export function LessonList({
  courses,
  onCompleteLesson,
  isProcessing,
  onSelectLesson
}: LessonListProps) {
  if (courses.length === 0) {
    return (
      <div className="panel">
        <p>Chưa có khóa học nào. Hãy kiểm tra lại sau!</p>
      </div>
    );
  }

  return (
    <div className="panel">
      <header>
        <div>
          <p className="eyebrow">Trường học</p>
          <h2>Khóa học & Bài học</h2>
        </div>
      </header>
      <div className="course-stack">
        {courses.map((course) => (
          <div key={course.khoaHocId} className="course-card">
            <div>
              <h3>{course.tenKhoaHoc}</h3>
              {course.moTa && <p className="muted">{course.moTa}</p>}
            </div>
            <ul>
              {course.lessons.map((lesson) => (
                <li key={lesson.baiHocId}>
                  <span>
                    {lesson.thuTu && <strong className="muted">#{lesson.thuTu} </strong>}
                    {lesson.tenBaiHoc}
                  </span>
                  <div className="lesson-actions">
                    <button
                      className="mini"
                      onClick={() => onSelectLesson?.(lesson.baiHocId)}
                      disabled={isProcessing}
                    >
                      Học ngay
                    </button>
                  </div>
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    </div>
  );
}

