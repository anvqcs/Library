import { useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import { Button, Table } from 'react-bootstrap';
import classNames from 'classnames/bind';

import Pagination from '~/components/Pagination';
import * as applicationUserService from '~/services/applicationUserService';
import styles from './User.module.css';

const cx = classNames.bind(styles);

function User() {
  const [members, setUsers] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 10;
  useEffect(() => {
    const fetchApi = async () => {
      const result = await applicationUserService.getAll(currentPage, pageSize);
      setUsers(result.items);
      setTotalPages(result.totalPages);
    };
    fetchApi();
  }, [currentPage]);

  return (
    <div className="mt-4">
      <h3 className="text-center mb-3">Library Users</h3>
      <Table
        striped
        bordered
        hover
        responsive
        className={cx('table', 'shadow-sm')}
      >
        <thead>
          <tr className="bg-primary text-white">
            <th>#</th>
            <th>Name</th>
            <th>Email</th>
            <th>Phone Number</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {members &&
            members.map((member, index) => (
              <tr key={member.id}>
                <td>{(currentPage - 1) * pageSize + index + 1}</td>
                <td>{member.fullName}</td>
                <td>{member.userName}</td>
                <td>{member.phoneNumber}</td>
                <td>
                  {/* <Button className={cx('btn', 'btn__delete')}>Delete</Button> */}
                  <NavLink to={'/user-form/' + member.id}>
                    <Button className={cx('btn', 'btn__edit')}>Edit</Button>
                  </NavLink>
                </td>
              </tr>
            ))}
        </tbody>
      </Table>
      <Pagination
        currentPage={currentPage}
        maxPageButtons={5}
        totalPages={totalPages}
        setCurrentPage={setCurrentPage}
      />
    </div>
  );
}

export default User;
