import { useState, useEffect } from 'react';
import { Button } from 'react-bootstrap';
import classNames from 'classnames/bind';
import { format } from 'date-fns';

import { useAuth } from '~/contexts/AuthContext';
import Pagination from '~/components/Pagination';
import Search from './Search';
import * as bookService from '~/services/bookService';
import * as borrowRecordsService from '~/services/borrowRecordsService';
import styles from './Home.module.css';

const cx = classNames.bind(styles);
function Home() {
  const { user } = useAuth();
  const [books, setBooks] = useState([]);
  const [filterData, setFilterData] = useState({});
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 8;

  useEffect(() => {
    const fetchApi = async () => {
      const result = await bookService.getWithPagination(
        currentPage,
        pageSize,
        {
          ...filterData,
        }
      );
      setBooks(result.items);
      setTotalPages(result.totalPages);
    };
    fetchApi();
  }, [currentPage, filterData]);
  const handleSearch = data => {
    setCurrentPage(1);
    setFilterData(data);
  };
  const handleBorrow = book => {
    if (window.confirm('Are you sure you want to borrow this book?')) {
      const fetchApi = async () => {
        const borrowItem = {
          bookId: book.id,
          applicationUserId: user.userId,
          rentalCost: book.rentalPrice,
        };
        await borrowRecordsService.add(borrowItem);

        const result = await bookService.getWithPagination(
          currentPage,
          pageSize,
          {
            ...filterData,
          }
        );
        setBooks(result.items);
        setTotalPages(result.totalPages);
      };
      fetchApi();
    }
  };
  return (
    <div>
      <h3 className={cx('title')}>Books</h3>
      <div className={cx('search')}>
        <Search onSubmit={handleSearch} />
      </div>
      <div className={cx('container')}>
        <div className={cx('grid')}>
          {books &&
            books.map(book => (
              <div key={book.id} className={cx('grid__item')}>
                <h3 className={cx('item__name')}>{book.title}</h3>
                <p className={cx('item__description')}>{book.description}</p>
                <p className={cx('item__price')}>
                  Rental Price: {book.rentalPrice}
                </p>
                <p className={cx('item__stock')}>Stock: {book.quantity}</p>

                <Button variant="primary" onClick={() => handleBorrow(book)}>
                  Borrow
                </Button>
              </div>
            ))}
        </div>
      </div>
      <Pagination
        currentPage={currentPage}
        maxPageButtons={5}
        totalPages={totalPages}
        setCurrentPage={setCurrentPage}
      />
    </div>
  );
}

export default Home;
