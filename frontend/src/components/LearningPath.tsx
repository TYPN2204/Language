import { useEffect, useState } from 'react';
import type { CourseDto, LessonDto, LessonProgressDto } from '../types/gameplay';
import { GameplayApi } from '../api/gameplay';
import './LearningPath.css';

interface LearningPathProps {
  courses: CourseDto[];
  hocSinhId: number;
  onSelectLesson: (lessonId: number) => void;
}

interface LessonWithProgress extends LessonDto {
  progress?: LessonProgressDto;
  isUnlocked: boolean;
  isMastered: boolean;
}

interface CourseWithLessons extends CourseDto {
  lessonsWithProgress: LessonWithProgress[];
}

export function LearningPath({ courses, hocSinhId, onSelectLesson }: LearningPathProps) {
  const [progress, setProgress] = useState<Record<number, LessonProgressDto>>({});
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadProgress = async () => {
      try {
        const progressData = await GameplayApi.getProgress(hocSinhId);
        setProgress(progressData);
      } catch (error) {
        console.error('Failed to load progress:', error);
      } finally {
        setIsLoading(false);
      }
    };

    loadProgress();
  }, [hocSinhId]);

  // Tính toán trạng thái khóa/mở khóa cho từng bài học
  const calculateUnlockStatus = (
    allLessons: LessonWithProgress[],
    currentIndex: number
  ): boolean => {
    if (currentIndex === 0) return true; // Bài đầu tiên luôn mở

    // Kiểm tra bài trước đó đã thông thạo chưa (SoLanHoanThanh >= 2)
    const previousLesson = allLessons[currentIndex - 1];
    return previousLesson?.isMastered ?? false;
  };

  // Flatten tất cả lessons từ tất cả courses để tính unlock
  const allLessonsFlat: LessonWithProgress[] = [];
  courses.forEach((course) => {
    course.lessons.forEach((lesson) => {
      const progressData = progress[lesson.baiHocId];
      allLessonsFlat.push({
        ...lesson,
        progress: progressData,
        isMastered: progressData?.isMastered ?? false,
        isUnlocked: false // Sẽ tính sau
      });
    });
  });

  // Tính unlock cho từng lesson
  allLessonsFlat.forEach((lesson, index) => {
    lesson.isUnlocked = calculateUnlockStatus(allLessonsFlat, index);
  });

  // Chuyển đổi courses thành coursesWithLessons
  let lessonIndex = 0;
  const coursesWithLessons: CourseWithLessons[] = courses.map((course) => {
    const lessonsWithProgress: LessonWithProgress[] = [];
    for (let i = 0; i < course.lessons.length; i++) {
      lessonsWithProgress.push(allLessonsFlat[lessonIndex]);
      lessonIndex++;
    }

    return {
      ...course,
      lessonsWithProgress
    };
  });

  if (isLoading) {
    return (
      <div className="learning-path-container">
        <div className="loading-message">Đang tải con đường học tập...</div>
      </div>
    );
  }

  return (
    <div className="learning-path-container">
      <div className="learning-path-scroll">
        <div className="learning-path-content">
          {/* Con đường SVG uốn lượn */}
          <svg className="path-line" viewBox="0 0 2000 400" preserveAspectRatio="none">
            <path
              d="M 0,200 Q 250,100 500,200 T 1000,200 T 1500,200 T 2000,200"
              stroke="#4CAF50"
              strokeWidth="8"
              fill="none"
              strokeLinecap="round"
            />
          </svg>

          {/* Các trạm chủ đề và cổng cấp độ */}
          <div className="stations-container">
            {coursesWithLessons.flatMap((course, courseIndex) => {
              const elements = [
                <div key={course.khoaHocId} className="course-station">
                  <TopicStation
                    course={course}
                    courseIndex={courseIndex}
                    onSelectLesson={onSelectLesson}
                  />
                </div>
              ];

              // Thêm cổng cấp độ sau mỗi 3 trạm
              if ((courseIndex + 1) % 3 === 0 && courseIndex < coursesWithLessons.length - 1) {
                const isUnlocked = coursesWithLessons
                  .slice(0, courseIndex + 1)
                  .every((c) => c.lessonsWithProgress.every((l) => l.isMastered));
                
                elements.push(
                  <LevelGate
                    key={`gate-${courseIndex}`}
                    gateIndex={Math.floor((courseIndex + 1) / 3)}
                    position={200 + (courseIndex + 1) * 400}
                    isUnlocked={isUnlocked}
                  />
                );
              }

              return elements;
            })}
          </div>
        </div>
      </div>
    </div>
  );
}

interface TopicStationProps {
  course: CourseWithLessons;
  courseIndex: number;
  onSelectLesson: (lessonId: number) => void;
}

function TopicStation({ course, courseIndex, onSelectLesson }: TopicStationProps) {
  const stationX = 200 + courseIndex * 400; // Khoảng cách giữa các trạm

  return (
    <div
      className="topic-station"
      style={{ left: `${stationX}px` }}
    >
      {/* Trạm chủ đề (hình tròn lớn) */}
      <div className="station-circle">
        <div className="station-icon">📚</div>
        <div className="station-label">{course.tenKhoaHoc}</div>
      </div>

      {/* Các vệ tinh bài học xung quanh */}
      <div className="lesson-satellites">
        {course.lessonsWithProgress.map((lesson, lessonIndex) => {
          // Tính góc để đặt vệ tinh xung quanh trạm
          const angle = (lessonIndex / course.lessonsWithProgress.length) * 2 * Math.PI;
          const radius = 120;
          const satelliteX = Math.cos(angle) * radius;
          const satelliteY = Math.sin(angle) * radius;

          return (
            <LessonNode
              key={lesson.baiHocId}
              lesson={lesson}
              x={satelliteX}
              y={satelliteY}
              onSelect={onSelectLesson}
            />
          );
        })}
      </div>
    </div>
  );
}

interface LessonNodeProps {
  lesson: LessonWithProgress;
  x: number;
  y: number;
  onSelect: (lessonId: number) => void;
}

function LessonNode({ lesson, x, y, onSelect }: LessonNodeProps) {
  const getNodeClass = () => {
    if (!lesson.isUnlocked) return 'lesson-node locked';
    if (lesson.isMastered) return 'lesson-node mastered';
    if (lesson.progress?.isCompleted) return 'lesson-node completed';
    return 'lesson-node available';
  };

  const getNodeIcon = () => {
    if (!lesson.isUnlocked) return '🔒';
    if (lesson.isMastered) return '⭐';
    if (lesson.progress?.isCompleted) return '✓';
    return '📖';
  };

  return (
    <div
      className={getNodeClass()}
      style={{
        transform: `translate(${x}px, ${y}px)`
      }}
      onClick={() => {
        if (lesson.isUnlocked) {
          onSelect(lesson.baiHocId);
        }
      }}
      title={lesson.tenBaiHoc}
    >
      <div className="lesson-node-icon">{getNodeIcon()}</div>
      <div className="lesson-node-label">
        {lesson.thuTu ? `#${lesson.thuTu}` : ''}
      </div>
      {lesson.progress?.soLanHoanThanh && lesson.progress.soLanHoanThanh > 0 && (
        <div className="lesson-node-progress">
          {lesson.progress.soLanHoanThanh}/2
        </div>
      )}
    </div>
  );
}

interface LevelGateProps {
  gateIndex: number;
  position: number;
  isUnlocked: boolean;
}

function LevelGate({ gateIndex, position, isUnlocked }: LevelGateProps) {
  return (
    <div
      className={`level-gate ${isUnlocked ? 'unlocked' : 'locked'}`}
      style={{ left: `${position}px` }}
    >
      <div className="gate-icon">{isUnlocked ? '🚪' : '🔒'}</div>
      <div className="gate-label">Cấp độ {gateIndex + 1}</div>
      <div className="gate-subtitle">{isUnlocked ? 'Đã mở' : 'Đang khóa'}</div>
    </div>
  );
}

