import { useState, useCallback, useMemo, useEffect } from 'react';
import debounce from 'lodash/debounce';

import { api } from '../../services/api';
import { ApplicationStatus, type JobApplication, SortKey } from '../../types';

import type { FilterState, TextFilterState, SortConfig } from './types';

export const useJobApplications = () => {
	const [page, setPage] = useState(1);
	const [totalPages, setTotalPages] = useState(1);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);
	const [applications, setApplications] = useState<JobApplication[]>([]);
	const [filters, setFilters] = useState<FilterState>({
		status: undefined,
		company: '',
		position: '',
	});

	const [textFilters, setTextFilters] = useState<TextFilterState>({
		company: '',
		position: '',
	});

	const [sortConfig, setSortConfig] = useState<SortConfig>({
		key: SortKey.DateApplied,
		direction: 'desc',
	});

	const debouncedTextFilters = useMemo(
		() => debounce((textFilters: TextFilterState) => {
			setFilters(
				prev => ({
					...prev,
					...textFilters
				})
			);
		}, 500),
		[]
	);

	const fetchApplications = useCallback(async () => {
		setLoading(true);
		setError(null);
		try {
			const response = await api.getApplications({
				...filters,
				sortBy: sortConfig.key,
				sortDirection: sortConfig.direction,
				page,
			});

			// Update applications and pages
			setApplications(response.data);
			setTotalPages(response.pages);
			if (page > response.pages) {
				setPage(1);
			}
		} catch (err) {
			setError('Failed to fetch applications!');
			console.error('Error fetching applications:', err);
		} finally {
			setLoading(false);
		}
	}, [filters, sortConfig, page]);

	useEffect(() => {
		fetchApplications();
	}, [fetchApplications]);

	const handleFilterChange = useCallback((field: keyof FilterState) =>
		(value: string | ApplicationStatus | null) => {
			console.log('Filter change:', { prev: filters, field, value });
			if (field === 'status') {
				setFilters(
					prev => ({
						...prev,
						status: value as ApplicationStatus | undefined,
					})
				);
			} else {
				const newTextFilters: TextFilterState = {
					...textFilters,
					[field]: value
				};
				setTextFilters(newTextFilters);
				debouncedTextFilters(newTextFilters);
			}
		},
		[textFilters, debouncedTextFilters, filters]
	);

	const handleSort = useCallback((key: SortKey) => {
		setSortConfig(prev => {
			const newDirection = key === prev.key
				? (prev.direction === 'asc' ? 'desc' : 'asc')
				: 'asc' as 'asc';

			return {
				key,
				direction: newDirection,
			};
		});
	}, []);

	return {
		applications,
		loading,
		error,
		totalPages,
		page,
		filters,
		textFilters,
		sortConfig,
		setPage,
		setFilters,
		setTextFilters,
		setSortConfig,
		fetchApplications,
		handleFilterChange,
		handleSort,
	};
};
