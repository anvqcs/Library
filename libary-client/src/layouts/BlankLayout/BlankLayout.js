import classNames from 'classnames/bind';

import styles from './BlankLayout.module.css';

const cx = classNames.bind(styles);

function BlankLayout({ children }) {
  return (
    <div className={cx('wrapper')}>
      <div className={cx('container')}>{children}</div>
    </div>
  );
}

export default BlankLayout;
