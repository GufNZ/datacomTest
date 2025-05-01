import React from 'react';

import { Pagination } from '@mui/material';

interface JobApplicationsPaginationProps {
	page: number;
	totalPages: number;
	onPageChange: (page: number) => void;
}

export const JobApplicationsPagination: React.FC<JobApplicationsPaginationProps> = ({
	page,
	totalPages,
	onPageChange,
}) => {
	return (
		totalPages > 1 && (
			<Pagination
				count={totalPages}
				page={page}
				onChange={(_, newPage) => onPageChange(newPage)}
				color="primary"
			/>
		)
	);
};
