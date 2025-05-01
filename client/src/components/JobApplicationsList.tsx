import React from 'react';

import { Alert, CircularProgress, Stack } from '@mui/material';

import { api } from '../services/api';
import type { JobApplication } from '../types';

import {
	JobApplicationsFilters,
	JobApplicationsPagination,
	JobApplicationsTable,
	useJobApplications,
} from './job-applications';

interface JobApplicationsListProps {
	onEdit: (application: JobApplication) => void;
}

export const JobApplicationsList: React.FC<JobApplicationsListProps> = ({ onEdit }) => {
	const {
		applications,
		loading,
		error,
		page,
		totalPages,
		filters,
		textFilters,
		sortConfig,
		setPage,
		fetchApplications,
		handleFilterChange,
		handleSort,
	} = useJobApplications();

	const handleDelete = async (application: JobApplication, cleanup?: () => void) => {
		try {
			await api.deleteApplication(application.id);
			await fetchApplications();
			cleanup?.();
		} catch (error) {
			console.error('Error deleting application:', error);
			cleanup?.();
		}
	};

	return (
		<Stack spacing={2}>
			<JobApplicationsFilters
				filters={filters}
				textFilters={textFilters}
				onFilterChange={handleFilterChange}
			/>
			{error && <Alert severity="error">{error}</Alert>}
			{loading ? (
				<CircularProgress sx={{ mt: 2 }}/>
			) : (
				<>
					<JobApplicationsTable
						applications={applications}
						onEdit={onEdit}
						onDelete={handleDelete}
						sortConfig={sortConfig}
						onSort={handleSort}
					/>
					<JobApplicationsPagination
						page={page}
						totalPages={totalPages}
						onPageChange={setPage}
					/>
				</>
			)}
		</Stack>
	);
};
