import React, { useState, useEffect } from 'react';
import classNames from 'classnames/bind';
import { Form, Button } from 'react-bootstrap';
import * as genreService from '~/services/genreService';
import styles from './Search.module.css';

const cx = classNames.bind(styles);

function Search({ onSubmit }) {
  const [genres, setGenres] = useState([]);
  const [formData, setFormData] = useState({
    title: '',
    author: '',
    genreId: '',
  });

  useEffect(() => {
    const fetchApi = async () => {
      const result = await genreService.getAll();
      setGenres(result);
    };
    fetchApi();
  }, []);
  const handleChange = e => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };
  const handleSubmit = e => {
    e.preventDefault();
    onSubmit(formData);
  };
  return (
    <Form onSubmit={handleSubmit} className={cx('form')}>
      <Form.Group className={cx('field', 'mb-3')}>
        <Form.Label htmlFor="title">Title: </Form.Label>
        <Form.Control
          type="text"
          name="title"
          value={formData.title}
          placeholder="Enter title"
          onChange={handleChange}
        />
      </Form.Group>

      <Form.Group className={cx('field', 'mb-3')}>
        <Form.Label htmlFor="author">Author: </Form.Label>
        <Form.Control
          type="text"
          name="author"
          value={formData.author}
          placeholder="Enter author"
          onChange={handleChange}
        />
      </Form.Group>

      <Form.Group className={cx('field', 'mb-3')}>
        <Form.Label htmlFor="genre">Genre: </Form.Label>
        <Form.Select
          name="genreId"
          value={formData.genreId}
          onChange={handleChange}
        >
          <option value="">Select a genre</option>
          {genres.map(genre => (
            <option key={genre.id} value={genre.id}>
              {genre.name}
            </option>
          ))}
        </Form.Select>
      </Form.Group>
      <Button variant="primary" type="submit" className={cx('btn-search')}>
        Search
      </Button>
    </Form>
  );
}

export default Search;
