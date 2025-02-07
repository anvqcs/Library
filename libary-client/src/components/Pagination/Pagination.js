import classNames from 'classnames/bind';
import styles from './Pagination.module.css';

const cx = classNames.bind(styles);

function Pagination({
  currentPage,
  maxPageButtons,
  totalPages,
  setCurrentPage,
}) {
  const getVisiblePages = () => {
    const half = Math.floor(maxPageButtons / 2);
    let start = Math.max(1, currentPage - half);
    let end = Math.min(totalPages, currentPage + half);

    // Adjust range when at the start or end
    if (currentPage <= half) {
      end = Math.min(totalPages, maxPageButtons);
    } else if (currentPage + half >= totalPages) {
      start = Math.max(1, totalPages - maxPageButtons + 1);
    }

    return Array.from({ length: end - start + 1 }, (_, i) => start + i);
  };
  return (
    <div className={cx('wrapper')}>
      <button
        className={cx('btn')}
        disabled={currentPage === 1}
        onClick={() => setCurrentPage(currentPage - 1)}
      >
        Previous
      </button>
      {getVisiblePages().map((page, i) => (
        <button
          key={page}
          onClick={() => setCurrentPage(page)}
          className={cx('btn', { active: currentPage === page })}
        >
          {page}
        </button>
      ))}
      <button
        className={cx('btn')}
        disabled={currentPage === totalPages}
        onClick={() => setCurrentPage(currentPage + 1)}
      >
        Next
      </button>
    </div>
  );
}

export default Pagination;
