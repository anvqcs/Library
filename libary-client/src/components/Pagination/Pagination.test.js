import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import Pagination from './Pagination';

describe('Pagination Component', () => {
  const setup = (props = {}) => {
    const utils = render(<Pagination {...props} />);
    const previousButton = screen.getByText('Previous');
    const nextButton = screen.getByText('Next');
    return {
      ...utils,
      previousButton,
      nextButton,
    };
  };

  test('renders pagination buttons correctly', () => {
    const { getByText } = setup({
      currentPage: 1,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage: jest.fn(),
    });

    expect(getByText('1')).toBeInTheDocument();
    expect(getByText('2')).toBeInTheDocument();
    expect(getByText('3')).toBeInTheDocument();
    expect(getByText('4')).toBeInTheDocument();
    expect(getByText('5')).toBeInTheDocument();
  });

  test('disables previous button on first page', () => {
    const { previousButton } = setup({
      currentPage: 1,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage: jest.fn(),
    });

    expect(previousButton).toBeDisabled();
  });

  test('disables next button on last page', () => {
    const { nextButton } = setup({
      currentPage: 10,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage: jest.fn(),
    });

    expect(nextButton).toBeDisabled();
  });

  test('calls setCurrentPage with correct page number when a page button is clicked', () => {
    const setCurrentPage = jest.fn();
    const { getByText } = setup({
      currentPage: 1,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage,
    });

    fireEvent.click(getByText('3'));
    expect(setCurrentPage).toHaveBeenCalledWith(3);
  });

  test('calls setCurrentPage with previous page number when previous button is clicked', () => {
    const setCurrentPage = jest.fn();
    const { previousButton } = setup({
      currentPage: 2,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage,
    });

    fireEvent.click(previousButton);
    expect(setCurrentPage).toHaveBeenCalledWith(1);
  });

  test('calls setCurrentPage with next page number when next button is clicked', () => {
    const setCurrentPage = jest.fn();
    const { nextButton } = setup({
      currentPage: 2,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage,
    });

    fireEvent.click(nextButton);
    expect(setCurrentPage).toHaveBeenCalledWith(3);
  });
  test('getVisiblePages returns correct pages when currentPage is in the middle', () => {
    const { getByText } = setup({
      currentPage: 5,
      maxPageButtons: 5,
      totalPages: 10,
      setCurrentPage: jest.fn(),
    });

    expect(getByText('3')).toBeInTheDocument();
    expect(getByText('4')).toBeInTheDocument();
    expect(getByText('5')).toBeInTheDocument();
    expect(getByText('6')).toBeInTheDocument();
    expect(getByText('7')).toBeInTheDocument();
  });
});
