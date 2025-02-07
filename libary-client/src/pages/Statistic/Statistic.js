import classNames from 'classnames/bind';
import CanvasJSReact from '@canvasjs/react-charts';

import * as borrowRecordsService from '~/services/borrowRecordsService';
import styles from './Statistic.module.css';
import { useEffect, useState } from 'react';

var CanvasJSChart = CanvasJSReact.CanvasJSChart;
const cx = classNames.bind(styles);
const monthNames = [
  'January',
  'February',
  'March',
  'April',
  'May',
  'June',
  'July',
  'August',
  'September',
  'October',
  'November',
  'December',
];
function Statistic() {
  const [year, setYear] = useState(2025);
  const [dataPoints, setDataPoints] = useState([]);
  useEffect(() => {
    const fetchApi = async () => {
      const result = await borrowRecordsService.statistic(year);
      const formattedData = result.map(record => ({
        label: record.month,
        y: record.totalBorrowed,
        totalCost: record.totalRentalCost,
      }));
      setDataPoints(formattedData);
    };
    fetchApi();
  }, [year]);
  const handleYearChange = event => {
    setYear(event.target.value);
  };
  const options = {
    animationEnabled: true,
    theme: 'light2',
    title: {
      text: 'Books Borrowed Per Month',
    },
    axisX: {
      title: 'Month',
    },
    axisY: {
      title: 'Quantity',
      includeZero: true,
      labelFormatter: function (e) {
        return Number.isInteger(e.value) ? e.value : '';
      },
    },
    data: [
      {
        type: 'column',
        toolTipContent: '{label}: {y} books, Total Cost: {totalCost}',
        dataPoints: dataPoints,
      },
    ],
  };
  return (
    <div className={cx('wrapper')}>
      <div>
        <label htmlFor="year-select">Choose a year:</label>
        <select id="year-select" value={year} onChange={handleYearChange}>
          <option value="2023">2023</option>
          <option value="2024">2024</option>
          <option value="2025">2025</option>
          <option value="2026">2026</option>
          <option value="2027">2027</option>
        </select>
      </div>
      <CanvasJSChart options={options} />
    </div>
  );
}

export default Statistic;
