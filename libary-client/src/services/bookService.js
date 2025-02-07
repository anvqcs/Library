import * as httpRequest from '~/utils/httpRequest';

export const getWithPagination = async (pageNumber, pageSize, searchData) => {
  try {
    const res = await httpRequest.get('books', {
      params: {
        pageNumber,
        pageSize,
        ...searchData,
      },
    });
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const getById = async id => {
  try {
    const res = await httpRequest.get(`books/${id}`);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const add = async formData => {
  try {
    const res = await httpRequest.post('books', formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const update = async formData => {
  try {
    const res = await httpRequest.put(`books`, formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const remove = async id => {
  try {
    const res = await httpRequest.remove(`books/${id}`);
    return res;
  } catch (error) {
    console.log(error);
  }
};
