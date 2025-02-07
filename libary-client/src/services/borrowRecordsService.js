import * as httpRequest from '~/utils/httpRequest';

export const getByUser = async (pageNumber, pageSize, searchData) => {
  try {
    const res = await httpRequest.get('borrow-records', {
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
export const statistic = async year => {
  try {
    const res = await httpRequest.get('borrow-records/statistic', {
      params: {
        year,
      },
    });
    return res;
  } catch (error) {
    console.log(error);
  }
};
export const add = async formData => {
  try {
    const res = await httpRequest.post('borrow-records', formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const update = async formData => {
  try {
    const res = await httpRequest.put(`borrow-records`, formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};
