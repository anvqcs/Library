import PropTypes from 'prop-types';
import classNames from 'classnames/bind';
import { Navigate } from 'react-router-dom';

import { useAuth } from '~/contexts/AuthContext';
import Header from '../components/Header';
import Sidebar from '../components/Sidebar';
import styles from './AdminLayout.module.css';

const cx = classNames.bind(styles);

function AdminLayout({ children }) {
  const { user } = useAuth();
  if (!user) {
    return <Navigate to="/login" />;
  }
  if (!user.role.includes('Administrator')) {
    return <Navigate to="/unauthorized" />;
  }
  return (
    <div className={cx('wrapper')}>
      <div className={cx('sidebar')}>
        <Sidebar IsAdmin />
      </div>
      <div className={cx('container')}>
        <Header />
        <div className={cx('content')}>
          <div className={cx('title')}>
            <h2>Library Management</h2>
          </div>
          {children}
        </div>
      </div>
    </div>
  );
}

AdminLayout.propTypes = {
  children: PropTypes.node.isRequired,
};
export default AdminLayout;
