import React from 'react';

import {
	FormControl,
	InputLabel,
	MenuItem,
	Select,
	Stack,
	TextField,
	Typography,
} from '@mui/material';

import { ApplicationStatus } from '../../types';

interface JobApplicationsFiltersProps {
	filters: {
		status: ApplicationStatus | undefined;
		company: string;
		position: string;
	};
	textFilters: {
		company: string;
		position: string;
	};
	onFilterChange: (field: keyof {
		status: ApplicationStatus | undefined;
		company: string;
		position: string;
	}) => (value: string | ApplicationStatus | null) => void;
}

export const JobApplicationsFilters: React.FC<JobApplicationsFiltersProps> = ({
	filters,
	textFilters,
	onFilterChange,
}) => {
	return (
		<Stack direction="row" spacing={2}>
			<FormControl fullWidth>
				<InputLabel>Status</InputLabel>
				<Select
					value={filters.status || ''}
					onChange={(event) => onFilterChange('status')(event.target.value)}
					label="Status"
				>
					<MenuItem value="">
					<Typography sx={{ fontWeight: 'bold' }}>All</Typography>
				</MenuItem>
					<MenuItem value={ApplicationStatus.Applied}>Applied</MenuItem>
					<MenuItem value={ApplicationStatus.Interview}>Interview</MenuItem>
					<MenuItem value={ApplicationStatus.Offer}>Offer</MenuItem>
					<MenuItem value={ApplicationStatus.Rejected}>Rejected</MenuItem>
				</Select>
			</FormControl>
			<TextField
				fullWidth
				label="Company"
				value={textFilters.company}
				onChange={(e) => onFilterChange('company')(e.target.value)}
				variant="outlined"
			/>
			<TextField
				fullWidth
				label="Position"
				value={textFilters.position}
				onChange={(e) => onFilterChange('position')(e.target.value)}
				variant="outlined"
			/>
		</Stack>
	);
};
