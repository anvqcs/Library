import { useState, useEffect } from 'react';
import { Table, Button } from 'react-bootstrap';
import classNames from 'classnames/bind';
import { format } from 'date-fns';

import Pagination from '~/components/Pagination';
import * as borrowRecordsService from '~/services/borrowRecordsService';
import styles from './Transaction.module.css';

const cx = classNames.bind(styles);

function Transaction() {
  const [borrowRecords, setBorrowRecords] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 8;

  useEffect(() => {
    const fetchApi = async () => {
      const result = await borrowRecordsService.getByUser(
        currentPage,
        pageSize,
        {}
      );
      setBorrowRecords(result.items);
      setTotalPages(result.totalPages);
    };
    fetchApi();
  }, [currentPage]);

  const handleReturn = item => {
    if (window.confirm('Are you sure you want to borrow this book?')) {
      const fetchApi = async () => {
        const putData = {
          ...item,
          isReturned: true,
        };
        await borrowRecordsService.update(putData);

        const result = await borrowRecordsService.getByUser(
          currentPage,
          pageSize,
          {}
        );
        setBorrowRecords(result.items);
      };
      fetchApi();
    }
  };
  return (
    <div className="mt-4">
      <h3 className={cx('text-center mb-3')}>My Books</h3>
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
            <th>Book</th>
            <th>Borrow Date</th>
            <th>Return Date</th>
            <th>Rental Cost</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {borrowRecords &&
            borrowRecords.map((borrowRecord, index) => (
              <tr key={borrowRecord.id}>
                <td>{index + 1}</td>
                <td>{borrowRecord.book}</td>
                <td>
                  {format(
                    new Date(borrowRecord.borrowDate),
                    'dd-MM-yyyy HH:mm'
                  )}
                </td>
                <td>
                  {borrowRecord.returnDate != null &&
                    format(
                      new Date(borrowRecord.returnDate),
                      'dd-MM-yyyy HH:mm'
                    )}
                </td>
                <td>{borrowRecord.rentalCost}</td>
                <td>
                  {!borrowRecord.returnDate && (
                    <Button onClick={() => handleReturn(borrowRecord)}>
                      Return
                    </Button>
                  )}
                </td>
              </tr>
            ))}
        </tbody>
      </Table>
      {totalPages > 0 && (
        <Pagination
          currentPage={currentPage}
          maxPageButtons={5}
          totalPages={totalPages}
          setCurrentPage={setCurrentPage}
        />
      )}
    </div>
  );
}

export default Transaction;
